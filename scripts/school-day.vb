Imports System.DateTime
Imports System.Net
Imports System.Text.RegularExpressions

function getSchoolDay(datevar as DateTime) as Integer
    dim y as string = datevar.ToString("yyyy")
    dim m as string = datevar.ToString("MM")
    dim d as string = datevar.ToString("dd")
    dim url as string = String.format("https://thrillshare-cmsv2.services.thrillshare.com/api/v4/o/19872/cms/events?start_date={0}-{1}-{2}&end_date={0}-{1}-{2}&view=cal-month&section_ids=335352&paginate=false&locale=en", y, m, d)

     dim webClient As New WebClient
     dim urlString As String = webClient.DownloadString(url)
     dim match as Match = Regex.Match(urlString, "Day ([1-6])", RegexOptions.IgnoreCase)
 
     if match.Success
       return Convert.ToInt32(match.Groups(1).ToString())
     else
       return 0
     end if
end function

sub Main(parm as object)
    const dayFeature as Integer = 719
    const elenaFeature as Integer = 1087
    const emmaFeature as Integer = 1088
    const tomorrowFeature as Integer = 1089

    dim today as DateTime = DateTime.Now

    'Today
    Dim schoolDay as Integer = getSchoolDay(today)
    if schoolDay > 0
        hs.WriteLog("SCHOOL", "Today is day " & schoolDay)
        hs.UpdateFeatureValueByRef(dayFeature, schoolDay)
        hs.UpdateFeatureValueStringByRef(dayFeature, "Day " & schoolDay)
        hs.UpdateFeatureValueByRef(elenaFeature, schoolDay)
        hs.UpdateFeatureValueByRef(emmaFeature, schoolDay)
    else
        hs.WriteLog("SCHOOL","No School Today")
        hs.UpdateFeatureValueByRef(dayFeature, 0)
        hs.UpdateFeatureValueStringByRef(dayFeature, "No School")
        hs.UpdateFeatureValueByRef(elenaFeature, 0)
        hs.UpdateFeatureValueByRef(emmaFeature, 0)
    end if

    'Tomorrow
    schoolDay = getSchoolDay(today.AddDays(1))
    if schoolDay > 0
        hs.WriteLog("SCHOOL", "Tomorrow is day " & schoolDay)
        hs.UpdateFeatureValueByRef(tomorrowFeature, schoolDay)
        hs.UpdateFeatureValueStringByRef(tomorrowFeature, "Day " & schoolDay)
    else
        hs.WriteLog("SCHOOL","No School Tomorrow")
        hs.UpdateFeatureValueByRef(tomorrowFeature, 0)
        hs.UpdateFeatureValueStringByRef(tomorrowFeature, "No School")
    end if

end sub
