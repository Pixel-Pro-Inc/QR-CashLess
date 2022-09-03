using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nager.Date;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BranchController : BaseApiController
    {
        private IPhotoService _photoService;
        public BranchController(IPhotoService photoService, IFirebaseServices firebaseServices):base(firebaseServices)
        {
            _photoService = photoService;
        }

        /// <summary>
        /// Takes in a <see cref="BranchDto"/> from the client so that the controller method have something to work with.
        /// <para> Then creates a branch if it meets the criteria and stores it in the database</para>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("register")]
        public async Task<ActionResult<BranchDto>> CreateBranch(BranchDto dto)
        {
            List<Branch> branches = new List<Branch>();
            branches = await _firebaseServices.GetData<Branch>("Branch");

            for (int i = 0; i < branches.Count; i++)
            {
                var item = branches[i];
                dto.Id = item.Id;

                while (item.Id == dto.Id)
                {
                    dto.Id = "rd" + new Random().Next(10000, 100000);
                    i = branches.Count + 1;
                }
            }

            Branch branch = new Branch()
            {
                Id = dto.Id,
                Location = dto.Location,
                Name = dto.Name,
                PhoneNumbers = GetPhoneNumbers_Int(dto.PhoneNumbers),
                OpeningTimes = GetDateTimes(dto.OpeningTime),
                ClosingTimes = GetDateTimes(dto.ClosingTime)
            };

            string path = dto.Img;

            if (path != null && path != "")
            {
                var result = await _photoService.AddPhotoAsync(path);

                if (result.Error != null) return BadRequest(result.Error.Message);

                branch.ImgUrl = result.SecureUrl.AbsoluteUri;
                branch.PublicId = result.PublicId;
            }        

            _firebaseServices.StoreData("Branch/" + dto.Id, branch);

            return dto;
        }

        List<int> GetPhoneNumbers_Int(List<string> numbers)
        {
            List<int> phoneNumbers = new List<int>();
            foreach (var number in numbers)
            {
                phoneNumbers.Add(Int32.Parse(number));
            }

            return phoneNumbers;
        }
        List<string> GetPhoneNumbers_String(List<int> numbers)
        {
            List<string> phoneNumbers = new List<string>();
            foreach (var number in numbers)
            {
                phoneNumbers.Add(number.ToString());
            }

            return phoneNumbers;
        }
        List<DateTime> GetDateTimes(object times)
        {
            List<DateTime> dateTimes = new List<DateTime>();

            var element = (JsonElement)times;

            for (int i = 0; i < element.GetArrayLength(); i++)
            {
                var time = element[i];

                dateTimes.Add(new DateTime(0001, 1, 1, GetTime(time)[0], GetTime(time)[1], 0));
            }
            
            return dateTimes;
        }

        int[] GetTime(object time)
        {
            var element = (JsonElement)time;

            var dateTime = element.GetString();

            int hours = Int32.Parse(dateTime.Substring(0, 2));

            int minutes = Int32.Parse(dateTime.Substring(3, 2));

            int[] values = { hours, minutes};

            return values;
        }

        [Authorize]
        [HttpPost("setclosingtime")]
        public async Task<ActionResult<DateTime>> CreateClosingTime(CloseTimeDto closeTimeDto)
        {
            var response = (await _firebaseServices.GetData<ClosingTime>("ClosingTime"));

            if (response.Count == 0)
                response.Add(new ClosingTime());

            object time = closeTimeDto.ClosingTime;

            var element = (JsonElement)time;

            response[0].ClosingTimeToday = new DateTime(0001, 1, 1, GetTime(element)[0], GetTime(element)[1], 0);
            response[0].EffectiveDate = DateTime.UtcNow.AddHours(2);


            _firebaseServices.StoreData("ClosingTime/" + 0, response[0]);

            return response[0].ClosingTimeToday;
        }
        public async Task<DateTime> GetClosingTime()
        {
            var response = (await _firebaseServices.GetData<ClosingTime>("ClosingTime"));

            if (response.Count == 0)
                return new DateTime();

            if (response[0].EffectiveDate.ToShortDateString() != System.DateTime.UtcNow.AddHours(2).ToShortDateString())
                return new DateTime();

            return response[0].ClosingTimeToday;
        }


        /// <summary>
        /// This is called from the client to get all the branches in the database
        /// 
        /// <para> The branches will come in as <see cref="BranchDto"/> instead of just the branch types</para>
        /// <para> NOTE: The branchDtos are made within the method</para>
        /// </summary>
        /// <returns> IEnumerable <see cref="BranchDto"/></returns>
        [HttpGet("getbranches")]
        public async Task<ActionResult<IEnumerable<BranchDto>>> GetBranches()
        {
            List<BranchDto> branches = new List<BranchDto>();

            var response = await _firebaseServices.GetData<Branch>("Branch");

            foreach (var item in response)
            {
                Branch branch = item;
                TimeSpan timeSpan = DateTime.UtcNow - item.LastActive;

                float x = (float)(timeSpan.TotalMinutes);

                DateTime prospectiveCloseTime = await GetClosingTime();

                BranchDto branchDto = new BranchDto()
                {
                    Id = item.Id,
                    Img = item.ImgUrl,
                    LastActive = x,
                    Location = branch.Location,
                    Name = branch.Name,
                    PhoneNumbers = GetPhoneNumbers_String(branch.PhoneNumbers),
                    OpeningTime = GetJsonElement(branch.OpeningTimes),
                    ClosingTime = await GetClosingTime() == new DateTime()? GetJsonElement(branch.ClosingTimes) : ConvertDateTimeToAngularTime(prospectiveCloseTime),
                    OpeningTimeTomorrow = GetJsonElement_Tomorrow(branch.OpeningTimes)
                };

                branches.Add(branchDto);
            }

            return branches;
        }
        JsonElement? GetJsonElement(List<DateTime> dateTimes)
        {
            //Get Todays Date And Opening/ Closing Time (Botswana)
            var currentDateTime = DateTime.UtcNow.AddHours(2);

            var publicHolidays = DateSystem.GetPublicHolidays(currentDateTime.Year, CountryCode.BW);

            DateTime dateTime = new DateTime();

            //Get time for day of week
            dateTime = dateTimes[GetDayOfWeek(currentDateTime.DayOfWeek)];

            //If is public holiday set to last item in array
            foreach (var item in publicHolidays)
            {
                if(item.Date.DayOfYear == currentDateTime.DayOfYear)
                {
                    dateTime = dateTimes[dateTimes.Count - 1];
                    break;
                }
            }

            if (dateTime == new DateTime())
                return null;

            return ConvertDateTimeToAngularTime(dateTime);
        }

        JsonElement ConvertDateTimeToAngularTime(DateTime dateTime)
        {
            var time = dateTime.Hour.ToString("00") + ":" + dateTime.Minute.ToString("00");

            var json = System.Text.Json.JsonSerializer.Serialize(time);

            var document = JsonDocument.Parse(json);

            var jsonElement = document.RootElement;

            return jsonElement;
        }
        JsonElement? GetJsonElement_Tomorrow(List<DateTime> dateTimes)
        {
            //Get Todays Date And Opening/ Closing Time (Botswana)
            var tomorrowDateTime = DateTime.UtcNow.AddHours(26);// 24 + 2 for tomorrow in the utc +2 time zone

            var publicHolidays = DateSystem.GetPublicHolidays(tomorrowDateTime.Year, CountryCode.BW);

            DateTime dateTime = new DateTime();

            //Get time for day of week
            dateTime = dateTimes[GetDayOfWeek(tomorrowDateTime.DayOfWeek)];

            //If is public holiday set to last item in array
            foreach (var item in publicHolidays)
            {
                if (item.Date.DayOfYear == tomorrowDateTime.DayOfYear)
                {
                    dateTime = dateTimes[dateTimes.Count - 1];
                    break;
                }
            }

            if (dateTime == new DateTime())
                return null;

            var time = dateTime.Hour.ToString("00") + ":" + dateTime.Minute.ToString("00");

            var json = System.Text.Json.JsonSerializer.Serialize(time);

            var document = JsonDocument.Parse(json);

            var jsonElement = document.RootElement;

            return jsonElement;
        }
        int GetDayOfWeek(DayOfWeek dayOfWeek)
        {
            var data = (DayOfWeek[])DayOfWeek.GetValues(typeof(DayOfWeek));
            List<DayOfWeek> properlyOrganizedDays = new List<DayOfWeek>();

            for (int i = 1; i < data.Length; i++)
            {
                properlyOrganizedDays.Add(data[i]);
            }

            properlyOrganizedDays.Add(data[0]);//Add Sunday as last day of week

            for (int i = 0; i < properlyOrganizedDays.Count; i++)
            {
                if (dayOfWeek == properlyOrganizedDays[i])
                    return i;
            }

            return 0;
        }

    }
}
