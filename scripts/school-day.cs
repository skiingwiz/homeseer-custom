using System;
using System.Net.WebClient;
using System.Text.Json;
using System.Text.Json.Nodes.JsonNode;

public int getSchoolDay(int year, int month, int day)
{
    string url = "https://thrillshare-cmsv2.services.thrillshare.com/api/v4/o/19872/cms/events?start_date="+year+"-"+month+"-"+day+"&end_date="+year+"-"+month+"-"+day+"&view=cal-month&section_ids=335352&paginate=false&locale=en";

    using (var webClient = new System.Net.WebClient()) {
        string urlString = webClient.DownloadString(url);
        var match = System.Text.RegularExpressions.Regex.Match(urlString, "Day ([1-6])", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        return (match.Success) ?
            Convert.ToInt32(match.Groups[1].ToString()) : 0;
    }
}

public int getDatePiece(DateTime date, string format)
{
    return Convert.ToInt32(DateTime.Now.ToString(format));
}

public object Main(object[] Parms)
{
    hs.WriteLog("SCHOOL","Script Running");
    int dayFeature = 719;
    int elenaFeature = 1087;
    int emmaFeature = 1088;
    int tomorrowFeature = 1089;

    var date = DateTime.Now;
    int year = getDatePiece(date, "yyyy");
    int month = getDatePiece(date, "MM");
    int day = getDatePiece(date, "dd");

    int schoolDay = getSchoolDay(year, month, day);
    if(schoolDay > 0) {
        hs.WriteLog("SCHOOL", "Today is day " + schoolDay);
        hs.UpdateFeatureValueByRef(dayFeature, schoolDay);
        hs.UpdateFeatureValueStringByRef(dayFeature, "Day " + schoolDay);
        hs.UpdateFeatureValueByRef(elenaFeature, schoolDay);
        hs.UpdateFeatureValueByRef(emmaFeature, schoolDay);
    } else {
        hs.WriteLog("SCHOOL","No School Today");
        hs.UpdateFeatureValueByRef(dayFeature, 0);
        hs.UpdateFeatureValueStringByRef(dayFeature, "No School");
        hs.UpdateFeatureValueByRef(elenaFeature, 0);
        hs.UpdateFeatureValueByRef(emmaFeature, 0);
    }

    date = date.AddDays(1);
    year = getDatePiece(date, "yyyy");
    month = getDatePiece(date, "MM");
    day = getDatePiece(date, "dd");

    schoolDay = getSchoolDay(year, month, day+1);
    if(schoolDay > 0) {
        hs.WriteLog("SCHOOL", "Tomorrow is day " + schoolDay);
        hs.UpdateFeatureValueByRef(tomorrowFeature, schoolDay);
        hs.UpdateFeatureValueStringByRef(tomorrowFeature, "Day " + schoolDay);
    } else {
        hs.WriteLog("SCHOOL","No School Tomorrow");
        hs.UpdateFeatureValueByRef(tomorrowFeature, 0);
        hs.UpdateFeatureValueStringByRef(tomorrowFeature, "No School");
    }


    return 0;
}
