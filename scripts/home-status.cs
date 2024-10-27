using System;
using System.Collections.Generic;

public object Main(object[] Parms)
{
    //Note: HomeSeer has a bug that doesn't allow the use of collections in this script
    //Dictionary<string, string> good_values_by_ref = new Dictionary<string, string>();
    //good_values_by_ref.Add(827, 23.0);

    var /*Dictionary<String, String>*/ categories = hs4.GetAllCategories();
    foreach(var /*KeyValuePair<string, string>*/ entry in categories)
    {
        if(entry.Value.StartsWith("Status-"))
        {
            //The number after the dash is the feature ref to be updated
            int statusRef = Convert.ToInt32(entry.Value.Remove(0, "Status-".Length));
            var /*List<ing>*/ catRefs = hs4.GetRefsByCategoryId(entry.Key);

            //Start the status at 0. If nothing in this category is on, that's what we'll
            //set the value to.
            int status_value = 0;
            foreach(int r in catRefs)
            {
                double val = hs4.GetDeviceByRef(r).Value;
    //            if(good_values_by_ref.ContainsKey(r)) {
                if(r == 827 || r == 957)
                {
    //                if(val != good_values_by_ref[r]) {
                    if(val != 23.0)
                    {
                        status_value = Math.Max(status_value, 1);
                    }
                }
                else if(val > 0)
                {
                    status_value = Math.Max(status_value, 1);
                    break;
                }
            }
            hs.UpdateFeatureValueByRef(statusRef, status_value);
            hs.UpdateFeatureValueStringByRef(statusRef, status_value == 0 ? "Green" : "Yellow");
            hs.WriteLog("StatusScript","Set Ref " + statusRef + " to " + status_value);
        }
    }

    return 0;
}
