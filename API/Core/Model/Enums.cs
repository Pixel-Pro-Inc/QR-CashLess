namespace API.Core.Model
{
    public class Enums
    {
        public enum restuarant
        {
            rodizio
        }
        /*
         So I don't want to use several bools to have for the different things I am putting below, it just made more sense
        to have an enum and have it store either a string or a number that would refer to each other. If enums don't work
        we can decide to use a small dictionary so we don't have to work too much. Cause bools are easy, but hard to manage
        practically
         */

        //These are the flavours of the chicken platters that are possible. There can only be one per platter
        public enum flavours
        {
            Peri_Peri,
            Mild_Peri_Peri,
            Lemon_and_Herb
        }
        //This is the way the meat will be cooked. They will be selected the same way as flavours
        public enum prepQuality
        {
            rare,
            medium,
            medium_well,
            well_done
        }
        //These can be bought extra but with platters you have them for free, so this is a necessary property to use
        public enum sauces
        {
            Lemon_Butter,
            Lemon_and_Garlic,
            Peri_Peri
        }
       
    }
}
