using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AssignAuto
/// </summary>
public class AssignAuto2
{

    public DataTable dtConfiguration;
    public DataTable dtClassTeacher;
    public DataTable dtTeacherHours;
    public DataTable dtSchoolHours;
    public DataTable dtAssign = new DataTable();
    public DataTable dtShPar;
    public DataTable dtKibua;
    public DataTable dtHourExtra;

    //public 
    public int MaxHourInShibutz;
    public int MinForPitzul;
    public int MinTeacherInmor;
    public int ConfigurationId;

    public AssignAuto2(DataSet ds)
    {

        dtConfiguration = ds.Tables[0];
        dtClassTeacher = ds.Tables[1];
        dtTeacherHours = ds.Tables[2];
        dtSchoolHours = ds.Tables[3];
        dtShPar = ds.Tables[4];
        dtKibua = ds.Tables[5];
        dtHourExtra = ds.Tables[6];


        dtAssign.Columns.Add("ConfigurationId");
        dtAssign.Columns.Add("TeacherId");
        dtAssign.Columns.Add("HourId", System.Type.GetType("System.Int32"));
        dtAssign.Columns.Add("HourTypeId");
        dtAssign.Columns.Add("ClassId");
        dtAssign.Columns.Add("ProfessionalId");
        dtAssign.Columns.Add("Hakbatza");
        dtAssign.Columns.Add("Ihud");




        foreach (DataRow row in dtKibua.Rows)
        {
            DataRow dr = dtAssign.NewRow();
            dr[0] = row["ConfigurationId"].ToString();
            dr[1] = row["TeacherId"].ToString();
            dr[2] = row["HourId"].ToString();
            dr[3] = 1;
            dr[4] = row["ClassId"].ToString(); 
            dr[5] = row["ProfessionalId"].ToString();
            dr[6] = row["Hakbatza"].ToString();
            dr[7] = row["Ihud"].ToString(); 


            dtAssign.Rows.Add(dr);

        }




        MaxHourInShibutz = ConvertToInt(dtConfiguration.Rows[0]["MaxHourInShibutz"]);
        MinForPitzul = ConvertToInt(dtConfiguration.Rows[0]["MinForPitzul"]);
        MinTeacherInmor = ConvertToInt(dtConfiguration.Rows[0]["MinTeacherInmor"]);
        ConfigurationId = ConvertToInt(dtConfiguration.Rows[0]["ConfigurationId"]);
    }

    public bool StartAssign()
    {
        //  DataRow[] drNotMehank = dtClassTeacher.Select("IsTeacher is null");

        // 

        foreach (DataRow row in dtClassTeacher.Rows)
        {
            int Hakbatza = ConvertToInt(row["Hakbatza"]);
            int Ihud = ConvertToInt(row["Ihud"]);

            //if (row["TeacherId"].ToString() == "32")
            //{


            //}
            //if (Hakbatza != 0 || Ihud != 0)
            //{

            //    DataRow[] drWithIhudHakbatza = dtAssign.Select("Hakbatza='" + Hakbatza + "'  and Ihud='" + Ihud + "'");
            //    if (drWithIhudHakbatza.Count() > 0)
            //        continue;
            //}

            int TotalTeacherInClass = ConvertToInt(row["TotalTeacherInClass"]);
            for (int i = 0; i < TotalTeacherInClass; i++)
            {

                //if (Hakbatza != 0 || Ihud != 0)
                //{
                //    AddRowToAssignHakbatzaIhud(row);
                //}
                //else
                //{
                    AddRowToAssign(row);
               // }
            }

           


        }

        return true;
       // SetShehyaPartani();
    }

    private void AddRowToAssignHakbatzaIhud(DataRow rowIhudHakbatza)
    {
        string Hakbatza = ConvertToInt(rowIhudHakbatza["Hakbatza"]).ToString();
        string Ihud = ConvertToInt(rowIhudHakbatza["Ihud"]).ToString();
        string MainTeacherId = rowIhudHakbatza["TeacherId"].ToString();
        string MainClassId = rowIhudHakbatza["ClassId"].ToString();
        string IsTwoHourHak = rowIhudHakbatza["IsTwoHour"].ToString();


     

        DataRow[] drHakbatzaIhud = dtClassTeacher.Select("Hakbatza='" + Hakbatza + "'  Or Ihud='" + Ihud + "'");

        DataTable drTeachers = GetDtTeacherHourWithOrderFreeDay(MainTeacherId, MainClassId, IsTwoHourHak); //dtTeacherHours.Select("TeacherId=" + TeacherId + " And IsOnlyShehya = 0");

        //if (drHakbatzaIhud.Count() == 4)
        //{


        //}

        foreach (DataRow teach in drTeachers.Rows)
        {
            int HourId = ConvertToInt(teach["HourId"]);//GetRandomFromTeacherHour(ref RowNumber, LastRowNumber);
            int HourOnly = HourId % 10;
            int Day = HourId / 10;


            int CounterOfValid = 0;
            string TeacherId = "";
            string ClassId = "";
            string TafkidId = "";
            string IsTeacher = "";
            string ProfessionalId = "";
            string IsTwoHour = "";

            foreach (DataRow row in drHakbatzaIhud)
            {

                TeacherId = row["TeacherId"].ToString();

                
              
                ClassId = row["ClassId"].ToString();
                TafkidId = row["TafkidId"].ToString();
                IsTeacher = row["IsTeacher"].ToString();
                ProfessionalId = row["ProfessionalId"].ToString();
                IsTwoHour = row["IsTwoHour"].ToString();
                int TotalTeacherInClass = ConvertToInt(row["TotalTeacherInClass"]);//.ToString();

                // אם מורה בכלל לא עובד
                if (!IsTeacherWorkInThisHour(TeacherId, HourId))
                {
                    break;
                }

                bool IsTeacherWorkInDay = false;

                if (TafkidId != "3" && IsTeacher != "True" && HourOnly <= MinTeacherInmor) // בודקים רק מורה מקצועי
                {
                    IsTeacherWorkInDay = GetIsTeacherWorkInDay(HourId, ClassId, Day);
                }

                bool IsClassAndTeacherHourFree = GetIsClassAndTeacherHourFree(HourId, ClassId, TeacherId, IsTeacher,
                  Day, TotalTeacherInClass,IsTwoHour);

                if (!IsTeacherWorkInDay && IsClassAndTeacherHourFree)
                {

                    CounterOfValid++;

                }
                else
                {

                    break;
                }


            }//end of loop 
            if (CounterOfValid == drHakbatzaIhud.Count())
            {
                foreach (DataRow item in drHakbatzaIhud)
                {
                    InsertToDbAndToDtAssign(item["TeacherId"].ToString(), HourId, 1, item["ClassId"].ToString(), item["ProfessionalId"].ToString(), Hakbatza, Ihud);
                }

                break;

            }




        }
    }

    private bool IsTeacherWorkInThisHour(string teacherId, int hourId)
    {
        DataRow[] drsTeacher = dtTeacherHours.Select("TeacherId='" + teacherId + "' and  HourId='"+ hourId + "'");

        if (drsTeacher.Count() > 0) return true;

        return false;
    }

    private void AddRowToAssign(DataRow row)
    {

        try
        {
            string TeacherId = row["TeacherId"].ToString();

           

            string ClassId = row["ClassId"].ToString();
            string Hakbatza = row["Hakbatza"].ToString();
            string TafkidId = row["TafkidId"].ToString();
            string Ihud = row["Ihud"].ToString();
            string IsTeacher = row["IsTeacher"].ToString();
            string ProfessionalId = row["ProfessionalId"].ToString();
            int TotalTeacherInClass = ConvertToInt(row["TotalTeacherInClass"]);//.ToString();
            string IsTwoHour = row["IsTwoHour"].ToString();

         



            DataTable drTeachers = GetDtTeacherHourWithOrderFreeDay(TeacherId, ClassId, IsTwoHour); //dtTeacherHours.Select("TeacherId=" + TeacherId + " And IsOnlyShehya = 0");

            foreach (DataRow teach in drTeachers.Rows)
            {


                int HourId = ConvertToInt(teach["HourId"]);//GetRandomFromTeacherHour(ref RowNumber, LastRowNumber);
                int HourOnly = HourId % 10;
                int Day = HourId / 10;
                bool IsTeacherWorkInDay = false;

                if (TafkidId != "3" && IsTeacher != "True" && HourOnly <= MinTeacherInmor) // בודקים רק מורה מקצועי
                {
                    IsTeacherWorkInDay = GetIsTeacherWorkInDay(HourId, ClassId, Day);
                }

                bool IsClassAndTeacherHourFree = GetIsClassAndTeacherHourFree(HourId, ClassId, TeacherId, IsTeacher,
                    Day, TotalTeacherInClass,IsTwoHour);

                if (!IsTeacherWorkInDay && IsClassAndTeacherHourFree)
                {

                    InsertToDbAndToDtAssign(TeacherId, HourId, 1, ClassId, ProfessionalId, Hakbatza, Ihud);


                    break;


                }

            }
        }
        catch (Exception ex)
        {



        }


    }

    private void InsertToDbAndToDtAssign(string TeacherId, int HourId, int HourTypeId, string ClassId, string ProfessionalId, string Hakbatza, string Ihud)
    {
        DataRow dr = dtAssign.NewRow();
        dr[0] = ConfigurationId;
        dr[1] = TeacherId;
        dr[2] = HourId;
        dr[3] = 1;
        dr[4] = ClassId;
        dr[5] = ProfessionalId;
        dr[6] = Hakbatza;
        dr[7] = Ihud;


        dtAssign.Rows.Add(dr);

        Dal.ExeSp("Assign_SetAssignAuto", ConfigurationId, TeacherId, HourId, 1, ClassId, ProfessionalId, Hakbatza, Ihud);

    }
    

    //פונקציה מחזירה את השעות של המורה בהתאם ליום החופשי של המורה
    private DataTable GetDtTeacherHourWithOrderFreeDay(string teacherId, string classId,string IsTwoHour)
    {

       

        int MainTeacherId = ConvertToInt((dtClassTeacher.Select("IsTeacher=1 And ClassId=" + classId))[0]["TeacherId"]);
        int FreeDay = ConvertToInt((dtClassTeacher.Select("IsTeacher=1 And ClassId=" + classId))[0]["FreeDay"]);

        DataTable drcollTeacherClass = dtTeacherHours.Clone();
        DataTable drcollFreeTeacherClass = dtTeacherHours.Clone();
        DataTable drcoll = dtTeacherHours.Clone();

        DataTable drcollMain = dtTeacherHours.Clone();


        DataRow[] drsTeacherClass;
        DataRow[] drsTeacher;
        // מביא שעות של יום חופשי של המחנך עצמו
        drsTeacherClass = dtTeacherHours.Select("TeacherId='" + MainTeacherId + "'");
        drsTeacher = dtTeacherHours.Select("TeacherId='" + teacherId + "'");// and  HourId > " + (FreeDay * 10) + " And HourId < " + (FreeDay * 10 + 10));

        foreach (DataRow row in drsTeacher)
        {

            bool IsiN = false;
            string HourId = row["HourId"].ToString();

            foreach (DataRow rowMainTeacher in drsTeacherClass)
            {
                if (HourId == rowMainTeacher["HourId"].ToString())
                {

                    drcoll.ImportRow(row);
                    
                     IsiN = true;
                    break;
                }
            }

            if (!IsiN)
            {

                if (ConvertToInt(HourId) / 10 == FreeDay)
                {
                    drcollFreeTeacherClass.ImportRow(row);
                }
                else
                {
                    drcollTeacherClass.ImportRow(row);
                }

            }

        }


        // אם זה 2 שעות תביא לפי הסדר
        if (IsTwoHour=="True")
        {

            // יום חופשי של המחנכת
            var rand2 = new Random();
            var result2 = drcollFreeTeacherClass.AsEnumerable().OrderBy(r => rand2.Next());
            foreach (DataRow row in drcollFreeTeacherClass.Rows)
            {
                drcollMain.ImportRow(row);
            }

            // שעות בהם המורה לא מלמדת
            var rand1 = new Random();
            var result1 = drcollTeacherClass.AsEnumerable().OrderBy(r => rand1.Next());
            foreach (DataRow row in drcollTeacherClass.Rows)
            {
                drcollMain.ImportRow(row);
            }



            // מביא את השעות של המורה רנדומלי
            // drsTeacher = dtTeacherHours.Select("TeacherId='" + teacherId + "' and  (HourId <" + (FreeDay * 10) + " Or HourId > " + (FreeDay * 10 + 10) + ")");
            var rand = new Random();
            var result = drcoll.AsEnumerable().OrderBy(r => rand.Next());
            foreach (DataRow row in drcoll.Rows)
            {

                drcollMain.ImportRow(row);

            }
        }else
        {
            // יום חופשי של המחנכת
            var rand2 = new Random();
            var result2 = drcollFreeTeacherClass.AsEnumerable().OrderBy(r => rand2.Next());
            foreach (DataRow row in result2)
            {
                drcollMain.ImportRow(row);
            }

            // שעות בהם המורה לא מלמדת
            var rand1 = new Random();
            var result1 = drcollTeacherClass.AsEnumerable().OrderBy(r => rand1.Next());
            foreach (DataRow row in result1)
            {
                drcollMain.ImportRow(row);
            }



            // מביא את השעות של המורה רנדומלי
            // drsTeacher = dtTeacherHours.Select("TeacherId='" + teacherId + "' and  (HourId <" + (FreeDay * 10) + " Or HourId > " + (FreeDay * 10 + 10) + ")");
            var rand = new Random();
            var result = drcoll.AsEnumerable().OrderBy(r => rand.Next());
            foreach (DataRow row in result)
            {

                drcollMain.ImportRow(row);

            }




        }

        return drcollMain;

        //  drsTeacher += dtTeacherHours.Select("TeacherId=" + teacherId + " and  HourId > " + (FreeDay * 10) + " And HourId < " + (FreeDay * 10 + 10));



    }

    private bool GetIsClassAndTeacherHourFree(int HourId, string ClassId, string TeacherId, string IsTeacher, 
        int Day, int TotalTeacherInClass,string IsTwoHour)
    {


        //if (IsTwoHour == "True")
        //{

        //}

        DataRow[] dr = dtAssign.Select("HourId=" + HourId + " and(ClassId=" + ClassId + " Or TeacherId=" + TeacherId + ")");
       // DataRow[] drKibua = dtKibua.Select("HourId=" + HourId + " and(ClassId=" + ClassId + " Or TeacherId=" + TeacherId + ")");

        if (dr.Count() > 0)
        {

            return false;
        }
        else
        {

            dr = dtAssign.Select("ClassId='" + ClassId + "' And TeacherId='" + TeacherId + "'  and HourId  >" + (Day * 10) + " And HourId <" + (Day * 10 + 10));
            DataRow[] drExtra = dtHourExtra.Select("ClassId='" + ClassId + "' And TeacherId='" + TeacherId + "'  and DayId='" + Day + "'");

            int MaxHourInDay = MaxHourInShibutz;

            if(drExtra.Count() > 0)
            {
                MaxHourInDay = ConvertToInt(drExtra[0]["HourExtra"].ToString());

            }




            if (dr.Count() >= MaxHourInDay && IsTeacher != "True")
            {
                return false;
            }
            else
            {  // אם יש לו רק 2 שעות בכיתה אל תשים אותם באותו יום...
                if (TotalTeacherInClass <= MinForPitzul && dr.Count() == MinForPitzul - 1 && IsTwoHour != "True")
                    return false;
                return true;
            }
            // return true;

        }

    }

    private bool GetIsTeacherWorkInDay(int HourId, string ClassId, int Day)
    {
        DataRow[] dr = dtClassTeacher.Select("ClassId='" + ClassId + "' And IsTeacher=1");
        if (dr.Count() == 0) return false;

        string ClassTeacherId = dr[0]["TeacherId"].ToString();

        dr = dtTeacherHours.Select("TeacherId='" + ClassTeacherId + "' And HourId=" + HourId);

        //בדיקה כמה שעות מורה משובץ ביום הזה
        //   DataRow[] drs = dtAssign.Select("TeacherId='" + ClassTeacherId + "' and HourId  >" + (Day * 10) + " And HourId <" + (Day * 10 + 10));




        if (dr.Count() > 0)
        {
            //  InsertToDbAndToDtAssign(ClassTeacherId, HourId, 1, ClassId, "", "", "");
            return true;
        }
        else
        {

            return false;
        }



    }

   // public DataTable dtRandomRows = new DataTable();
    private void SetShehyaPartani()
    {
      //  dtRandomRows = GetRandomDataTable(dtTeacherHours);

        //DataRow[] drShehyaPartani;

        //if (IsTeacher)
        //    drShehyaPartani = dtShPar.Select("TafkidId=1");
        //else
        //    drShehyaPartani = dtShPar.Select("TafkidId=2");



        foreach (DataRow row in dtShPar.Rows)
        {

            int CounterPartani = ConvertToInt(row["CounterPartani"]);
            int CounterShehya = ConvertToInt(row["CounterShehya"]);

            for (int i = 0; i < CounterPartani; i++)
            {
                AddShehyaPartaniAssign(row, 2);
            }


            for (int i = 0; i < CounterShehya; i++)
            {
                AddShehyaPartaniAssign(row, 3);
            }


        }

    }

    private void AddShehyaPartaniAssign(DataRow row, int HourTypeId)
    {
        string TeacherId = row["TeacherId"].ToString();
        string TafkidId = row["TafkidId"].ToString();


        DataRow[] drTeachers = dtTeacherHours.Select("TeacherId=" + TeacherId + " And IsOnlyShehya=0");
        foreach (DataRow teach in drTeachers)
        {
            int HourId = ConvertToInt(teach["HourId"]);//GetRandomFromTeacherHour(ref RowNumber, LastRowNumber);
            int HourOnly = HourId % 10;
            int Day = HourId / 10;

            bool IsFreeHour = GetIsTeacherHourFree(HourId, TeacherId, Day, HourTypeId, TafkidId, HourOnly);
            if (IsFreeHour)
            {
                DataRow dr = dtAssign.NewRow();
                dr[0] = ConfigurationId;
                dr[1] = TeacherId;
                dr[2] = HourId;
                dr[3] = HourTypeId;
                dr[4] = DBNull.Value;
                dr[5] = DBNull.Value;
                dr[6] = DBNull.Value;
                dr[7] = DBNull.Value;


                dtAssign.Rows.Add(dr);

                Dal.ExeSp("Assign_SetAssignAuto", ConfigurationId, TeacherId, HourId, HourTypeId, "", "", "", "");

                break;

            }



        }



    }

    private bool GetIsTeacherHourFree(int HourId, string TeacherId, int Day, int HourTypeId, string TafkidId, int HourOnly)
    {

      //  if (TafkidId == "1" && HourOnly == 1) return false;


        DataRow[] drs = dtAssign.Select("TeacherId='" + TeacherId + "'  and HourId  >" + (Day * 10) + " And HourId <" + (Day * 10 + 10));

        foreach (DataRow item in drs)
        {
            if (ConvertToInt(item["HourId"]) == HourId)
            {
                return false;
            }


        }

        drs = dtAssign.Select("HourTypeId='" + HourTypeId + "' And TeacherId='" + TeacherId + "'  and HourId  >" + (Day * 10) + " And HourId <" + (Day * 10 + 10));



        if (drs.Count() >= 2)// לא יותר משתיים ביום פרטני
        {
            return false;
        }

        return true;



    }




    private DataTable GetRandomDataTable(DataTable dtTeacherHours)
    {
        DataTable dt = dtTeacherHours;
        DataTable dtRandomRows = new DataTable("RandomTable");
        dtRandomRows = dt.Clone();
        Random rDom = new Random();
        int i = 0;
        for (int ctr = 0; ctr <= dtTeacherHours.Rows.Count; ctr++)
        {
            i = rDom.Next(0, dtTeacherHours.Rows.Count);

            // DataRow dr = dtRandomRows.NewRow();

            // dr = dt.Rows[i];
            dtRandomRows.ImportRow(dt.Rows[i]);

            // dtRandomRows.AcceptChanges();
        }

        return dtRandomRows;
    }





    //private int GetRandomFromTeacherHour(ref int RowNumber, int LastRowNumber)
    //{

    //    if (LastRowNumber == 0)
    //    {

    //        Random r = new Random();
    //        int j = r.Next(0, dtTeacherHours.Rows.Count);
    //        return ConvertToInt(dtTeacherHours.Rows[j]["HourId"]);
    //    }
    //    else
    //    {
    //       // return ConvertToInt(dtTeacherHours.Rows[j]["HourId"]);


    //    }


    //}

    private int ConvertToInt(object v)
    {
        int x;

        bool IsOk = Int32.TryParse(v.ToString(), out x);
        if (IsOk)
            return x;

        return 0;

    }
}