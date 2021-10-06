using System;
using System.Collections.Generic;

public object Main(object[] Parms)
{
    int status_value = 0;
    const int status_device_ref = 884;
    //Dictionary<string, string> good_values_by_ref = new Dictionary<string, string>();
    //good_values_by_ref.Add(827, 23.0);

    var /*Dictionary<String, String>*/ categories = hs4.GetAllCategories();
    foreach(var /*KeyValuePair<string, string>*/ entry in categories)
    {
        if(entry.Value.StartsWith("Status-"))
        {
            int catVal = Convert.ToInt32(entry.Value.Remove(0, "Status-".Length));
            var /*List<ing>*/ catRefs = hs4.GetRefsByCategoryId(entry.Key);

            foreach(int r in catRefs)
            {
                double val = hs4.GetDeviceByRef(r).Value;
    //            if(good_values_by_ref.ContainsKey(r)) {
                if(r == 827) {
    //                if(val != good_values_by_ref[r]) {
                    if(val != 23.0) {
                        status_value = Math.Max(status_value, catVal);
                    }
                }
                else if(val > 0)
                {
                    status_value = Math.Max(status_value, catVal);
                    break;
                }
            }
        }
    }

    hs.UpdateFeatureValueByRef(status_device_ref, status_value);
    hs.WriteLog("StatusScript","Set Status to " + status_value);
    return 0;
}
