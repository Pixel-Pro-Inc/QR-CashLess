using API.Application.Exceptions;
using API.Application.Interfaces;
using AutoMapper;
using RodizioSmartKernel.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace API.Infrastructure.Services
{
    /// <summary>
    /// This is service that will calculate all the necessary information that needs to be sent to and fro the debtor.
    /// It will deal with all the billing buisness logic.
    /// </summary>
    public class BillingServices : BaseService, IBillingServices
    {
        IFirebaseServices _IFirebaseServices;
        private readonly IMapper _mapper;
        private readonly IReportServices _reportServices;
        private readonly IAccountService _accountService;

        public BillingServices(IFirebaseServices firebaseServices, IMapper mapper, IReportServices reportServices, IAccountService accountService)
        {
            _IFirebaseServices = firebaseServices;
            _mapper = mapper;
            _reportServices = reportServices;
            _accountService = accountService;
        }

        #region Properties

        

        private float WebsiteCost { get; set; }
        private float DomainCost { get; set; }
        private float OtherServicesCost { get; set; }

        /// <summary>
        /// These are the sales of the month that the BillingServices will work with. I uses the month of the <see cref="_User.DuePaymentDate"/>
        /// </summary>
        private float branchSales { get; set; }

        /// <summary>
        /// This is the cost of a single SMS depending on the API we use
        /// </summary>
        private float SMSUnitCost { get; set; }
        /// <summary>
        /// This will the discount we are affording them expressed as a decimal percentage eg. 0.05 for 5% discount
        /// </summary>
        private float DiscountPercent { get; set; }

        private static float _LicensingPercent { get; set; }
        /// <summary>
        /// This is the percentage that we charge for our services as a percent of their sales.
        /// <para> It will be expressed as a decimal eg 0.0475 as 4.75%</para>
        /// </summary>
        private static float LicensingPercent { get { return _LicensingPercent; } set { _LicensingPercent= 0.0475f; } }

        private static float _TaxRate { get; set; }
        /// <summary>
        /// This is the tax rate of the company in the country. The default is 14% expressed as 0.14f
        /// </summary>
        private float TaxRate { get { return _TaxRate; } set { _TaxRate = 0.14f; } }

        /// <summary>
        /// Describes the invoice in general
        /// <para> Would be used if a custom description is needed</para>
        /// </summary>
        private string invoiceDescription { get; set; }
        /// <summary>
        /// Describes the invoice in general
        /// <para> Would be used if a custom description is needed</para>
        /// </summary>
        private string otherServicesDescription { get; set; }


        /// <summary>
        /// This is what the billingService uses to represent the user. You can only set this property. 
        /// </summary>
        /// <remarks>
        /// The user will be set by whatever calls Billingservices, but it should not be accessed by anyone outside Billing. Hence its a private get. All the information
        /// needed that will be possible to get from billing will be handed off by the methods themselves
        /// </remarks>
        internal AdminUser _User { private get; set; }

        DateTime Today { get; set; }

        [Obsolete]
        public List<SMS> SmsList { get; set; }

        #endregion

        #region Methods

        //I thought, we could just store the value but then how will we accomodate for changes, We would have to have logic
        //to calculate those changes. It would be easier to simply calculate how much is due, each time, assuming a change has been made, (eg date,
        // ammount, number of branches being managed)
        // FIXME: This isn't how we calculate payment anymore. check the create invoice for the logic
        public float CalculateTotalPaymentDue()
        {
            // calculates the overdue span by finding how much longer Today is after due date
            TimeSpan overdueSpan = Today-_User.DuePaymentDate;
            // If today is before due date the user owes nothing
            if (Today.CompareTo(_User.DuePaymentDate) < 0) return 0;

            // divides it by a month of Febuary, 28 days and takes the whole number
            int NumerofOverdueMonths = (int)overdueSpan.TotalDays / DateTime.DaysInMonth(Today.Year, 2);
            
            float result=0;
            // gets the current charge per branch and multiples it by the NumerofOverdueMonths
            foreach (var branch in _User.BilledBranchIds)
            {
                result += branch.Value * NumerofOverdueMonths;
            }
            // returns the product of the result and the monthly fee per branch
            return result;
        }
        public void SetCurrentDate(DateTime date) => Today = date;
        public DateTime DueDate() => _User.DuePaymentDate;

        // REFACTOR: Consider having this set to a specific period of time like, 8:00 in the morning
        public void SetDueDate(DateTime DueDate) => _User.DuePaymentDate = DueDate;
        public void SetDueDate(DateTime Today, int MonthsfromDate) => _User.DuePaymentDate=Today.AddMonths(MonthsfromDate);

        // REFACTOR: Consider having this set to a specific period of time like, 8:00 in the morning
        public bool isPastDueDate(DateTime Today) => Today > DueDate();
        public void SetUser(AppUser user)
        {
            // We could find a way to check if the user is an admin as a subclass but this is more effective and shorter
            if (!user.Admin)
            {
                throw new UnBillableUserException("You aren't properly set up to be billed by Pixel Pro. Please contact them to recover Services",
                  new InvalidCastException("This user isn't billed at the end of the month/ isn't a AdminUser"));
            }

            _User=_mapper.Map<AdminUser>(user);
        }
       
        // FIXME: Please test to see if the Document is produced as anticipate
        public void CreateInvoice()
        {
            // takes in the word document and replaces the necessary variables
            CreateDocFromTemplate();

            // convert the word to a PDf
            // returns the pdf so it can be attached to the email
            throw new NotImplementedException();
        }

        // REFACTOR: Consider this being in a WordDocumentService
        /// <summary>
        /// This takes in a word document that has bookmarks of all the texts we want to replace.
        /// <para> Then it replaces those texts with chosen values and outputs a new <see cref="Microsoft.Office.Interop.Word.Document"/></para>
        /// </summary>
        private Microsoft.Office.Interop.Word.Document CreateDocFromTemplate()
        {
            // Creates word application to make word doc
            Microsoft.Office.Interop.Word.Application wordApp = null;
            wordApp = new Microsoft.Office.Interop.Word.Application();
            wordApp.Visible = true;

            // This opens the document using the one within the specified path
            Microsoft.Office.Interop.Word.Document wordDoc = wordApp.Documents
                .Open(Configuration["templateDocumentDirectories:QRCashlessInvoiceTemplate"]);

            // Make a document we can work and edit with
            Microsoft.Office.Interop.Word.Document workingDoc = wordDoc;

            // this was supposed to be the variable "0000" in the invoice but that is too much work for now
            //int invoicesToday= 0;

            SetWordValues(workingDoc);

            // We don't know what kind of error could be thrown like, directory doesn't exist or corrupted file or any of the sort
            try
            {
                // TESTING: This is where I expect a file to be generated and analysed for accuracy
                string folderpath = Configuration["templateDocumentDirectories:folderpath"];
                string date = DateTime.Today.ToString("dd.MM.yyyy");
                string fileName = Path.Combine(folderpath, $"PixelPro Invoice-{DateTime.Now.ToPixelProInvoiceFormat() + "0000" + "-00"}", date);

                Directory.CreateDirectory(fileName);

                workingDoc.SaveAs2(fileName);
                workingDoc.Close();
            }
            catch (Exception)
            {
                // TODO: Add a logger here
                throw;
            }

            // closes word application
            wordApp.Quit();
            wordApp = null;

            return workingDoc;
        }

        /// <summary>
        /// This sets the variable values into the bookmarks in the word Template
        /// <para> We use bookmarks cause that seems to be the easiest way of replacing certain text with what we want</para>
        /// </summary>
        /// <param name="workingDoc"></param>
        private void SetWordValues(Microsoft.Office.Interop.Word.Document workingDoc)
        {
            AdminUser recipient = _mapper.Map<AdminUser>(_User);

            // REFACTOR: This is a good place to put the calculate values
            //Calculating properties
            float _licensingcost=0f;
            float _SupportCost=0f;
            float _SMSCost = 0f;

            float total = 0f;

            #region Invoice View variables
            // REFACTOR: Consider doing something different here cause, its just too much
            foreach (Microsoft.Office.Interop.Word.Bookmark bookmark in workingDoc.Bookmarks)
            {
                //I tried using the for loop but it wouldn't work cause its a dictionary so the '[i]' wn't work
                switch (bookmark.Name)
                {
                    case "_invoiceNumber":
                        setBookmarkValue(bookmark, DateTime.Now.ToPixelProInvoiceFormat() + "0000" + "-00");
                        break;
                    case "_dateofIssue":
                        setBookmarkValue(bookmark, DateTime.Now.ToPixelProForwardSlashFormat());
                        break;
                    case "_billedToName":
                        setBookmarkValue(bookmark, recipient.Fullname);
                        break;
                    case "_billedToEmail":
                        setBookmarkValue(bookmark, recipient.Email);
                        break;
                    case "_billedToAddress":
                        setBookmarkValue(bookmark, recipient.Address);
                        break;
                    case "_SupportUnitCost":
                        // TODO: Put logic for supportCost when you can
                        setBookmarkValue(bookmark, "0.00");
                        break;
                    case "_SupportCost":
                        // TODO: Put logic for supportCost when you can
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", _SupportCost));
                        break;
                    case "_PixelProRate":
                        setBookmarkValue(bookmark, (LicensingPercent * 100).ToString());
                        break;
                    case "_LicensingUnitCost":
                        //Sets the format to two decimal places
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", LicensingPercent));
                        break;
                    case "_LicensingCost":
                        float sales = branchSales;
                        _licensingcost = sales * LicensingPercent;
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", _licensingcost));
                        break;
                    case "_SMSUnitCost":
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", SMSUnitCost));
                        break;
                    case "_SMSCost":
                        // @Abel: Mr Billy has 172 SMS to pay for but I'm leaving the automatic logic here
                        int smsNumber = SmsList.Count;
                        _SMSCost = SMSUnitCost * smsNumber;
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", _SMSCost));
                        break;
                    case "_WebsiteUnitCost":
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", WebsiteCost));
                        break;
                    case "_WebsiteCost":
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", WebsiteCost));
                        break;
                    case "_DomainUnitCost":
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", DomainCost));
                        break;
                    case "_DomainCost":
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", DomainCost));
                        break;
                    case "_OtherServicesUnitCost":
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", OtherServicesCost));
                        break;
                    case "_OtherServicesCost":
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", OtherServicesCost));
                        break;
                    case "_InvoiceTotal":
                        total = _SupportCost + _licensingcost + _SMSCost + WebsiteCost + DomainCost + OtherServicesCost;
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", total));
                        break;
                    case "_SubTotal":
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", total));
                        break;
                    case "_Discount":
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", DiscountPercent));
                        break;
                    case "_TaxRate":
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", TaxRate));
                        break;
                    case "_TaxAmount":
                        // multiples the invoiceTotal/subTotal by the  tax rate 
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", TaxRate * total));
                        break;
                    case "_TotalAmount":
                        // Adds the tax amount to the subtotal
                        setBookmarkValue(bookmark, String.Format("{0:0.##}", (TaxRate * total) + total));
                        break;
                    case "_invoiceDescription":
                        invoiceDescription =
                            $"This invoice is in regards to the services provided to Rodizio Express in the month of {DateTime.Now.Month} {DateTime.Now.Year}";
                        setBookmarkValue(bookmark, invoiceDescription);
                        break;
                    case "_otherDescription":
                        setBookmarkValue(bookmark, otherServicesDescription);
                        break;

                    default:
                        break;
                }
            }
            #endregion

        }

        /// <summary>
        /// Sets the value of the bookmark using <see cref="Microsoft.Office.Interop.Word.Range"/>
        /// <para> I'm pretty sure the syntax is
        /// <code>
        ///  Microsoft.Office.Interop.Word.Range rng = bkm.Range;
        /// rng.Text = Value;
        /// </code>
        /// <para>Otherwise there isn't anything special here</para>
        /// </para>
        /// </summary>
        /// <param name="bkm"></param>
        /// <param name="Value"></param>
        private static void setBookmarkValue(Microsoft.Office.Interop.Word.Bookmark bkm, string Value)
        {
            Microsoft.Office.Interop.Word.Range rng = bkm.Range;
            rng.Text = Value; 
        }

        // TODO: This is to be called by the automatic call. I'm thinking maybe it should be done by the hosting, azure and only 4 times a month
        // @Abel: If you don' set this method, nothing will work
        /// <summary>
        /// Checks for the Users with overdue branches and reports the findings
        /// </summary>
        /// <remarks>It does this by sending them the bill while also sending the developers an email
        /// about the option of disabling functionality temporally.</remarks>
        /// <returns></returns>
        private async Task ReportOverDueStatements()
        {
            List<AdminUser> OwingUsers = new List<AdminUser>();

            List<AdminUser> adminUsers = await _accountService.GetAdminAccounts();
            // Collects all the users who owe 
            foreach (var user in adminUsers)
            {

                // REFACTOR: Consider having this set to a specific period of time like, 8:00 in the morning
                if (user.DuePaymentDate < System.DateTime.Today)
                    OwingUsers.Add(user);
                continue;
            }

            // foreach OwingUser it should make a BillingDto and send it to the BillingController
            foreach (var user in OwingUsers)
            {

                BilledUserDto dto = new BilledUserDto()
                {
                    Username = user.UserName
                };
                await new BillingController(this,_accountService, _reportServices,_IFirebaseServices).BillSender(dto);

            }

        }

        public async void SetSalesinUsersBranch()
        {
            DateTime Firstday = Today.FirstDayOfMonth();
            DateTime LastDay = Today.LastDayOfMonth();

            // tries to get the sales. If it fails it should not change the next due date and should throw an error
            try
            {
                foreach (string branch in _User.branchId)
                {
                    branchSales += await _reportServices.GetSalesAmountinTimePeriod(new ReportDto() { StartDate = Firstday, EndDate = LastDay, BranchId = branch });
                }
            }
            catch(IncorrectPeriodInputException)
            {
                // TODO: Add logger
                throw;
            }
            catch (Exception)
            {
                // this in the case the the database call flops or whatever unexpected
                throw;
            }

            //Resets the _User.DuePaymentDate to the next month
            SetDueDate(Today);

        }

        /// <summary>
        /// Returns the SMS. You can tell that I ran out of time  
        /// </summary>
        /// <param name="smses"></param>
        public void SetSMSSent(List<SMS> smses) => SmsList = smses;

        #endregion

    }
}
