namespace ERPNchr.HtmlHelpers
{
    public static class NationalIdExtensions
    {
        public static DateTime? GetBirthDate(this string nationalId)
        {
            if (string.IsNullOrEmpty(nationalId) || nationalId.Length < 7)
                return null;

            int centuryFlag = int.Parse(nationalId.Substring(0, 1));
            int century = (centuryFlag == 2) ? 1900 : 2000;

            int year = century + int.Parse(nationalId.Substring(1, 2));
            int month = int.Parse(nationalId.Substring(3, 2));
            int day = int.Parse(nationalId.Substring(5, 2));

            return new DateTime(year, month, day);
        }

        public static int? GetAge(this string nationalId)
        {
            var birthDate = nationalId.GetBirthDate();
            if (birthDate == null)
                return null;

            var today = DateTime.Today;
            int age = today.Year - birthDate.Value.Year;

            if (birthDate > today.AddYears(-age))
                age--;

            return age;
        }
    }

}
