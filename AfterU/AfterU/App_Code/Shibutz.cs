using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Shibutz
/// </summary>
public class Shibutz
{
    public Configuration Config;
    public DataTable dt;
    public List<HourSchool> School;
    public List<HakbatzaIhud> HakbatzaIhudList;

    public Shibutz(DataSet ds)
    {

        InitConfigurationData(ds);

        this.dt = ds.Tables[0];
        School = new List<HourSchool>();
        BuildAllSchoolFromDB();

    }

    private void InitConfigurationData(DataSet ds)
    {
        Config = new Configuration(ds.Tables[1].Rows[0]);

        foreach (DataRow row in ds.Tables[2].Rows)
        {
            HourExtra ex = new HourExtra(row);
            Config.AddHourExtra(ex);
        }

        HakbatzaIhudList = new List<HakbatzaIhud>();
        foreach (DataRow row in ds.Tables[3].Rows)
        {
            HakbatzaIhud hi = new HakbatzaIhud(row);
            HakbatzaIhudList.Add(hi);
        }





    }

    private void BuildAllSchoolFromDB()
    {
        foreach (DataRow row in dt.Rows)
        {
            string TeachList = row["TeachList"].ToString();
            if (string.IsNullOrEmpty(TeachList)) continue;

            HourSchool hs = new HourSchool(row);


            string[] Teacher = TeachList.Split(',');


            bool IsHasHakbatza = false;
            bool IsHasIhud = false;
            foreach (var item in Teacher)
            {
                ClassTeacher ct = new ClassTeacher(item, hs.ClassId);

                if (ct.Ihud != 0) IsHasIhud = true;
                if (ct.Hakbatza != 0) IsHasHakbatza = true;
                hs.AddClassTeacher(ct);

            }

            hs.IsHasHakbatza = IsHasHakbatza;
            hs.IsHasIhud = IsHasIhud;
            this.School.Add(hs);



        }

        UpdateAllNaatzim();

        this.School = School.OrderByDescending(x => x.IsHasIhud && x.IsHasHakbatza).ToList();
        // this.School = School.OrderBy(x => x.TeacherOptional.Any(y=>y.IsTeacher).ToList();

    }

    private void UpdateAllNaatzim()
    {
        foreach (HourSchool hsObj in School)
        {
            string[] TeacherNaatz = hsObj.TeacherNaatz.Split(',');




            foreach (string TeacherId in TeacherNaatz)
            {
                if (!string.IsNullOrEmpty(TeacherId))
                {

                    int HakbatzaCount = TeacherNaatz.Count();

                    var HakbatzaIhud = hsObj.TeacherOptional.FindAll(x => x.TeacherId.ToString() == TeacherId && (x.Hakbatza != 0 || x.Ihud != 0));
                    int Hakbatza = 0;
                    int Ihud = 0;

                    if (HakbatzaCount > 1)
                    {
                        foreach (ClassTeacher ct in HakbatzaIhud)
                        {
                            int CurrentHakbatza = ct.Hakbatza;
                            int CurrentIhud = ct.Ihud;

                            var HakbatzaIhudList = hsObj.TeacherOptional.FindAll(x => x.Hakbatza == CurrentHakbatza && x.Ihud == CurrentIhud);

                            if (HakbatzaIhudList.Count() == HakbatzaCount)
                            {
                                Hakbatza = HakbatzaIhudList[0].Hakbatza;
                                Ihud = HakbatzaIhudList[0].Ihud;

                            }

                        }
                    }

                    UpdateSchoolObject(hsObj, Helper.ConvertToInt(TeacherId), hsObj.Day, hsObj.Hour, hsObj.ClassId, Hakbatza, Ihud);

                }
            }

        }

        //var TeacherNaatzHakbatza = School.FindAll(x => x.SelectedTeacherIds.Count > 1);

        //foreach (HourSchool item in TeacherNaatzHakbatza)
        //{
        //    foreach (int TeacherIdSelect in item.SelectedTeacherIds)
        //    {
        //       // var ctObj = item.TeacherOptional.Contains();
        //    }

        //    //
        //}

    }
    public void StartShibutz()
    {
        // 94 צחי
        // 98 קוקת
        // 95 עדי
        // 96 שילה
        // 97 מיכל


        int Stam = 0;

        foreach (HourSchool hsObj in School)
        {
            int ClassId = hsObj.ClassId;
            int Day = hsObj.Day;
            int Hour = hsObj.Hour;

            if (ClassId == 2060 && Day == 2 && Hour == 3)
            {



            }

            if (hsObj.SelectedTeacherIds.Any()) continue;

            var optTeacher = hsObj.TeacherOptional.FindAll(x => x.TeacherHoursInClass != 0);


            if (optTeacher.Count > 0)
            {


                List<TempTeachersObj> TempTeachers = new List<TempTeachersObj>();

                foreach (ClassTeacher ctObj in optTeacher)
                {





                    int TeacherId = ctObj.TeacherId;

                    //if (TeacherId == 81 && (hsObj.HourId == 15 || hsObj.HourId == 21 || hsObj.HourId == 31 || hsObj.HourId == 44 || hsObj.HourId == 57 || hsObj.HourId == 25))
                    //{ }

                    int TeacherFree = 0;
                    int TeacherHourWork = 0;
                    int TeacherInDay = 0;
                    // int LastHourInDay = 0;

                    GetTeacherStatus(ref TeacherFree, ref TeacherHourWork, ref TeacherInDay, TeacherId, ClassId, Day, Hour);

                    TempTeachersObj tto = new TempTeachersObj();
                    tto.ClassId = ClassId;
                    tto.TeacherId = TeacherId;
                    tto.TeacherFree = TeacherFree;
                    tto.TeacherHourWork = TeacherHourWork;
                    tto.TeacherInDay = TeacherInDay;
                    tto.ctObj = ctObj;
                    tto.IsOk = true;

                    //// בדיקה האם קיימת הגבלה לשעות ביום למורה
                    //var HourExtraObj = Config.HourExtraList.FindAll(x => x.ClassId == ClassId && x.DayId == Day && x.TeacherId == TeacherId).FirstOrDefault();
                    //if (!ctObj.IsTeacher && TeacherInDay == Config.MaxHourInShibutz || (HourExtraObj != null && TeacherInDay == HourExtraObj.HourExtraHour))
                    //{
                    //    continue;
                    //}

                    //// אם הוא מלמד רק שעתיים אל תתן באותו יום
                    //if (!ctObj.IsTwoHour && ctObj.HardTeacherHoursInClass <= Config.MinForPitzul && TeacherInDay + 1 == Config.MinForPitzul)
                    //{
                    //    continue;

                    //}

                    //// אם מורה הוא מורה מקצועי בכיתה אחרת אל תכניס אותו במקום הכיתה שלו 
                    //var FirstHourForTeacher = School.FindAll(x => x.HourId == hsObj.HourId && x.ClassId != ClassId && x.Hour == 1 && x.TeacherOptional.Any(y => y.TeacherId == TeacherId && y.IsTeacher)).FirstOrDefault();
                    //if (Hour == 1 && FirstHourForTeacher != null)
                    //{
                    //    continue;
                    //}


                    //if (IsFutureTeacherLast(tto, hsObj) || ctObj.TeacherHoursInClass == 0)
                    //{
                    //    continue;
                    //}

                    // if (ctObj.TeacherHoursInClass > 0)

                    TempTeachers.Add(tto);


                }

                if (TempTeachers.Count > 0)
                {



                    var ObjTeacher = new TempTeachersObj();

                    var ObjHakbatzaIhud = TempTeachers.FindAll(x => x.ctObj.Hakbatza != 0 || x.ctObj.Ihud != 0);

                    // var ObjIhud = TempTeachers.FindAll(x => x.ctObj.Ihud != 0).FirstOrDefault();
                    var ObjTwoHour = TempTeachers.FindAll(x => x.ctObj.IsTwoHour).FirstOrDefault();

                    // מציאת מורה שזו השעה האחרונה שהוא יכול להשתבץ
                    var ObjLast = TempTeachers.FindAll(x => x.TeacherFree == 1).FirstOrDefault();

                    // מציאת מורה שמה שנשאר לו שווה למה שהוא עוד צריך
                    var ObjLastEmpty = TempTeachers.FindAll(x => x.TeacherHourWork - x.TeacherFree == 0).FirstOrDefault();

                    // אם מדובר במחנכת
                    var ObjTeacherFirst = TempTeachers.FindAll(x => x.ctObj.IsTeacher && Hour <= Config.MinTeacherInmor).FirstOrDefault();

                    var ObjKarev = TempTeachers.FindAll(x => x.ctObj.TafkidId == 3 && (x.TeacherHourWork - x.TeacherFree == 0 || x.TeacherFree == 1)).FirstOrDefault();




                    if (ObjKarev != null)
                    {
                        ObjTeacher = ObjKarev;
                    }

                    else if (ObjTeacherFirst != null)
                    {
                        ObjTeacher = ObjTeacherFirst;

                    }


                    else if (ObjHakbatzaIhud.Count > 0 && ObjHakbatzaIhud != null)
                    {

                        bool IsInsert = false;
                        foreach (TempTeachersObj item in ObjHakbatzaIhud)
                        {


                            if (IsHakIhudSet(1, item.ctObj.Hakbatza) && IsHakIhudSet(2, item.ctObj.Ihud))
                            {

                                IsInsert = SelectTeacherInMainHakbatzaIhud(ClassId, Day, Hour, item, hsObj, TempTeachers);
                                if (IsInsert) break;
                            }
                        }

                        if (IsInsert)
                        {
                            continue;
                        }
                        else
                        {

                            TempTeachers.RemoveAll(x => x.ctObj.Hakbatza != 0 || x.ctObj.Ihud != 0);

                            if (TempTeachers.Count == 0)
                                continue;
                            ObjTeacher = TempTeachers.Random();

                        }

                    }

                    else if (ObjLast != null)
                    {
                        ObjTeacher = ObjLast;

                    }

                    else if (ObjLastEmpty != null)
                    {
                        ObjTeacher = ObjLastEmpty;


                    }

                    else
                    {
                        //  continue;
                        ObjTeacher = TempTeachers.Random();//TempTeachers.OrderBy(x=>x.TeacherFree).FirstOrDefault();//
                    }


                    //***************************************************************8

                    SelectTeacherInMainRegular(ClassId, Day, Hour, ObjTeacher, hsObj);

                }
                else
                {

                }


            }
            else
            {
                Stam++;

            }




        }




        //*******************************************

        //  UpdateTeacherWithoutClassNewNew();
        // UpdateTeacherWithoutClassNew();

        //  DeleteChooseTeacherFromHour(96, 24);
       // DeleteChooseTeacherFromHour(96, 25);
        //School.FindAll(x => x.SelectedTeacherIds.Contains(-1)).ToList().ForEach(c => c.SelectedTeacherIds.Remove(c.SelectedTeacherIds[0]));

        //for (int i = 0; i < 5; i++)
        //{



        //School.FindAll(x => x.SelectedTeacherIds.Contains(-1)).ToList().ForEach(c=>c.SelectedTeacherIds.Remove(c.SelectedTeacherIds[0]));

        // UpdateTeacherWithoutClassNew();
        //}

        //*******************************************


        SqlConnection mySqlConnection = Dal.OpenConnection();

        var SchoolAll = School.FindAll(x => x.SelectedTeacherIds.Any() && string.IsNullOrEmpty(x.TeacherNaatz));
        foreach (var item in SchoolAll)
        {

            foreach (int Teacherid in item.SelectedTeacherIds)
            {

                //if(item.SelectedTeacherIds.Count>2)
                //{ }

                ClassTeacher ct = item.TeacherOptional.FindAll(x => x.TeacherId == Teacherid && x.Hakbatza == item.SelectedHakbatza && x.Ihud == item.SelectedIhud).FirstOrDefault();
                if (ct != null)
                    //if (ct.Hakbatza == 6)
                    //{ }
                    Dal.ExeSpBig(mySqlConnection, "Assign_SetAssignAuto", Config.ConfigurationId, Teacherid, item.HourId, 1, item.ClassId, ct.ProfessionalId, ct.Hakbatza, ct.Ihud);

            }



        }

        Dal.CloseConnection(mySqlConnection);



    }

    private void UpdateTeacherWithoutClassNewNew()
    {
        DateTime dtStart = DateTime.UtcNow;

        var EmptyHours = GetEmptyHour();

        foreach (var EmptyHour in EmptyHours)
        {
            // DateTime now = DateTime.UtcNow;
            // TimeSpan difference = now.Subtract(dtStart);

            //  var EmptyHour = GetEmptyHour();
            //   if (EmptyHour == null || difference.Minutes == 2) break;

            int ClassId = EmptyHour.ClassId;
            int HourId = EmptyHour.HourId;
            bool IsInsert = false;

            //foreach (ClassTeacher ct in EmptyHour.TeacherOptional)
            //{

            //   IsInsert =  CheckifInsert(EmptyHour,ClassId, HourId,ct.TeacherId);
            //   if (IsInsert) break;
            //}

            //if (IsInsert) continue;

            foreach (ClassTeacher ct in EmptyHour.TeacherOptional)
            {

                //if (ExistTransfer(ClassId, HourId, ct.TeacherId))
                //{
                //    continue;
                //}
                IsInsert = ReplaceInsert(EmptyHour, ClassId, HourId, ct.TeacherId, false, ct.Hakbatza, ct.Ihud);

                if (IsInsert)
                {
                    // TempTransferObj tt = new TempTransferObj(HourId,ClassId,ct.TeacherId);
                    // TransferObjList.Add(tt);
                    break;
                }

                if (ct == EmptyHour.TeacherOptional.Last())
                {
                    // var RandomCT = EmptyHour.TeacherOptional.Random();

                    // IsInsert = ReplaceInsert(EmptyHour, ClassId, HourId, RandomCT.TeacherId, true);
                    EmptyHour.SelectedTeacherIds.Add(-1);


                }



            }



        }






    }

    private bool IsFutureTeacherLast(TempTeachersObj tto, HourSchool hsObj)
    {
        int TeacherId = tto.TeacherId;
        int HourId = hsObj.HourId;
        int ClassId = hsObj.ClassId;


        //// בדיקה האם אותו מועומד הוא אחרון באותה שעה בכיתה אחרת אז אל תבחר אותו...
        //var TeacherList = School.FindAll(x => x.HourId == HourId && x.ClassId != ClassId && x.TeacherOptional.Any(y => y.TeacherId == TeacherId)
        //                                 && !x.SelectedTeacherIds.Any());

        //foreach (HourSchool item in TeacherList)
        //{
        //    int TeacherCounter = item.TeacherOptional.Count;
        //    int TeacherWithNoHour = item.TeacherOptional.FindAll(x => x.TeacherHoursInClass == 0).ToList().Count();
        //    int TeacherSelectedHourinClass = (item.TeacherOptional.FindAll(x => x.TeacherId == TeacherId).FirstOrDefault()).TeacherHoursInClass;


        //    if(TeacherCounter-1== TeacherWithNoHour && TeacherSelectedHourinClass == 1)
        //    {
        //        return true;
        //    }

        //}



        // בדיקה האם אותו מועומד הוא אחרון באותה שעה בכיתה אחרת אז אל תבחר אותו...
        var TeacherList = School.FindAll(x => x.ClassId == ClassId && x.HourId != HourId && x.TeacherOptional.Any(y => y.TeacherId == TeacherId)
                                          && !x.SelectedTeacherIds.Any());

        foreach (HourSchool item in TeacherList)
        {
            int TeacherCounter = item.TeacherOptional.Count;
            int TeacherWithNoHour = item.TeacherOptional.FindAll(x => x.TeacherHoursInClass == 0).ToList().Count();
            int TeacherSelectedHourinClass = (item.TeacherOptional.FindAll(x => x.TeacherId == TeacherId).FirstOrDefault()).TeacherHoursInClass;


            if (TeacherCounter - 1 == TeacherWithNoHour && TeacherSelectedHourinClass == 1)
            {

                int TeacherCounterObj = hsObj.TeacherOptional.Count;
                int TeacherWithNoHourObj = hsObj.TeacherOptional.FindAll(x => x.TeacherHoursInClass == 0).ToList().Count();
                int TeacherSelectedHourinClassObj = (hsObj.TeacherOptional.FindAll(x => x.TeacherId == TeacherId).FirstOrDefault()).TeacherHoursInClass;


                if (TeacherCounterObj - 1 == TeacherWithNoHourObj && TeacherSelectedHourinClassObj == 1)
                {
                    return true;
                }

                return true;
            }

        }



        return false;

        //var TeacherFreeList = School.FindAll(x => x.ClassId == ClassId && x.TeacherOptional.Any(y => y.TeacherId == TeacherId) && !x.SelectedTeacherIds.Any());//.SelectMany(y => y.TeacherOptional.);  // && x.TeacherOptional. && !x.SelectedTeacherIds.Any());


    }

    List<TempTransferObj> TransferObjList = new List<TempTransferObj>();

    private void UpdateTeacherWithoutClassNew()
    {

        DateTime dtStart = DateTime.UtcNow;

        var EmptyHours = GetEmptyHour();

        // לפנות את המורה מהיכן שתפוסה.
        // להחליף את הפינוי בשעה
        // 


        foreach (HourSchool hs in EmptyHours)
        {
            bool IsOk = false;
            var NoTeacherAssigns = GetNoTeacherAssign(hs.ClassId);
            foreach (ClassTeacher ct in hs.TeacherOptional)
            {


                // מקרים
                // בדיקת שעה ריקה שיש אחד ממנה הוא בעצם ממתין ורק תפוס במקום אחר
                var TeacherMeshubatz = School.FindAll(x => NoTeacherAssigns.Any(m => m.TeacherId == ct.TeacherId) && x.TeacherOptional.Any(y => y.TeacherId == ct.TeacherId) && x.HourId == hs.HourId && x.SelectedTeacherIds.Contains(ct.TeacherId)).FirstOrDefault();

                if (TeacherMeshubatz != null)
                {

                    IsOk = SwapHourInClass(TeacherMeshubatz.ClassId, ct.TeacherId, hs.HourId);

                    if (IsOk)
                    {

                        UpdateSchoolObject(hs, ct.TeacherId, hs.Day, hs.Hour, hs.ClassId, 0, 0);
                    }

                }



            }


            if (!IsOk)
            {
                //החלפות בתוך הכיתה עצמה
                foreach (ClassTeacher ct in hs.TeacherOptional)
                {
                    // בדיקה אם פנוי בשעה הזו
                    var TeacherMeshubatz = School.FindAll(x => x.TeacherOptional.Any(y => y.TeacherId == ct.TeacherId) && x.HourId == hs.HourId && x.SelectedTeacherIds.Contains(ct.TeacherId)).FirstOrDefault();

                    if (TeacherMeshubatz == null)
                    {
                        // כל השיבוצים של המורה בכיתה
                        var AllShibutzForTeacherOptional = School.FindAll(x => x.ClassId == hs.ClassId && x.SelectedTeacherIds.Contains(ct.TeacherId)).ToList();

                        foreach (ClassTeacher ctopt in NoTeacherAssigns)
                        {
                            // שעות חופשיות של המורה שממתין
                            List<HourSchool> AllFreeForTeacherOptional = GetHourFreeTeacher(ctopt.TeacherId);

                            //בדיקה אם יש חפיפה בין השעות של המורה לבין שעות חופשיות של המורה הממתין
                            var ExistSwapHours = AllFreeForTeacherOptional.Where(x => AllShibutzForTeacherOptional.Any(y => y.HourId == x.HourId)).FirstOrDefault();

                            if (ExistSwapHours != null)
                            {
                                // מחיקה של המורה שאפשר להחליף
                                DeleteChooseTeacherFromHour(ct.TeacherId, ExistSwapHours.HourId);

                                // הכנסה של המורה לשעה הריקה
                                UpdateSchoolObject(hs, ct.TeacherId, hs.Day, hs.Hour, hs.ClassId, 0, 0);

                                // הכנסה לשעה שהורדנו את המורה שפנוי
                                UpdateSchoolObject(ExistSwapHours, ctopt.TeacherId, ExistSwapHours.Day, ExistSwapHours.Hour, ExistSwapHours.ClassId, 0, 0);


                            }


                        }
                    }
                }

            }

        }




        //foreach (ClassTeacher ct in NoTeacherAssigns)
        //{
        //   // var FreeForTeacher = School.FindAll(x =>x.TeacherOptional.Any(y => y.TeacherId == ct.TeacherId) && x.).ToList();


        //   var FreeForTeacher = School.FindAll(x =>x.TeacherOptional.Any(y=>y.TeacherId==ct.TeacherId) && !x.SelectedTeacherIds.Contains(ct.TeacherId)).ToList();

        //}

    }

    private List<HourSchool> GetHourFreeTeacher(int TeacherId)
    {

        List<HourSchool> FreeTeacher = new List<HourSchool>();

        int FirstClassId = School[0].ClassId;
        var TempSchool = School.Where(x => x.ClassId == FirstClassId).ToList();

        foreach (var item in TempSchool)
        {
            var TeacherExistsList = School.FindAll(x => x.HourId == item.HourId && x.TeacherOptional.Any(y => y.TeacherId == TeacherId) && x.SelectedTeacherIds.Contains(TeacherId)).FirstOrDefault();
            if (TeacherExistsList == null)
            {

                FreeTeacher.Add(item);

            }


        }


        return FreeTeacher;


    }

    private bool SwapHourInClass(int ClassId, int TeacherId, int HourId)
    {
        var ReplaceHour = School.FindAll(x => x.ClassId == ClassId && x.HourId == HourId).FirstOrDefault();
        // מוצא את כל השיבוצים שבהם יכול להשתבץ המורה וכבר קיים שם מישהו אחר
        var AllClassShibutz = School.FindAll(x => x.ClassId == ClassId && x.TeacherOptional.Any(y => y.TeacherId == TeacherId) && x.SelectedTeacherIds.Any() && !x.SelectedTeacherIds.Contains(TeacherId)).ToList();
        foreach (HourSchool TeacherInClass in AllClassShibutz)
        {
            // המורה שמשובץ באותה שעה
            var ReplaceTeacherId = TeacherInClass.SelectedTeacherIds[0];

            // האם המורה פנוי בשעה הזו
            var ReplaceFreeTeacher = School.FindAll(x => x.HourId == HourId && x.TeacherOptional.Any(m => m.TeacherId == ReplaceTeacherId) && x.SelectedTeacherIds.Contains(ReplaceTeacherId)).FirstOrDefault();


            if (ReplaceFreeTeacher == null)
            {
                var FreeTeacher = School.FindAll(x => x.HourId == TeacherInClass.HourId && x.TeacherOptional.Any(m => m.TeacherId == TeacherId) && x.SelectedTeacherIds.Contains(TeacherId)).FirstOrDefault();
                if (FreeTeacher == null)
                {
                    DeleteChooseTeacherFromHour(ReplaceTeacherId, TeacherInClass.HourId);
                    DeleteChooseTeacherFromHour(TeacherId, ReplaceHour.HourId);

                    UpdateSchoolObject(TeacherInClass, TeacherId, TeacherInClass.Day, TeacherInClass.Hour, TeacherInClass.ClassId, 0, 0);
                    UpdateSchoolObject(ReplaceHour, ReplaceTeacherId, ReplaceHour.Day, ReplaceHour.Hour, ReplaceHour.ClassId, 0, 0);
                    return true;

                }



            }


        }


        return false;
    }

    // מורים ממתינים ללא שיבוץ
    private List<ClassTeacher> GetNoTeacherAssign(int ClassId)
    {
        var NoTeacherAssign = School.SelectMany(x => x.TeacherOptional).Where(y => y.ClassId == ClassId && y.TeacherHoursInClass > 0).ToList();
        List<ClassTeacher> NoTeacherAssigns = new List<ClassTeacher>();

        List<int> InnerClassId = new List<int>();
        foreach (ClassTeacher item in NoTeacherAssign)
        {
            if (!InnerClassId.Contains(item.ClassId))
            {


                NoTeacherAssigns.Add(item);
                InnerClassId.Add(item.ClassId);

            }


        }
        return NoTeacherAssigns;
    }

    // שעות ריקות משיבוץ
    private List<HourSchool> GetEmptyHour()
    {
        var EmptyHour = School.FindAll(x => x.SelectedTeacherIds.Count == 0).ToList();

        return EmptyHour;

    }





    //var EmptyHours = GetEmptyHour();

    //foreach (var EmptyHour in EmptyHours)
    //{

    //    //DateTime now = DateTime.UtcNow;
    //    //TimeSpan difference = now.Subtract(dtStart);

    //    //var EmptyHour = GetEmptyHour();
    //    //if (EmptyHour == null || difference.Minutes == 2) break;

    //    int ClassId = EmptyHour.ClassId;
    //    int HourId = EmptyHour.HourId;


    //    bool IsInsert = false;

    //    //foreach (ClassTeacher ct in EmptyHour.TeacherOptional)
    //    //{

    //    //   IsInsert =  CheckifInsert(EmptyHour,ClassId, HourId,ct.TeacherId);
    //    //   if (IsInsert) break;
    //    //}

    //    //if (IsInsert) continue;

    //    foreach (ClassTeacher ct in EmptyHour.TeacherOptional)
    //    {

    //        //if (ExistTransfer(ClassId, HourId, ct.TeacherId))
    //        //{
    //        //    continue;
    //        //}
    //        IsInsert = ReplaceInsert(EmptyHour, ClassId, HourId, ct.TeacherId, false, ct.Hakbatza, ct.Ihud);

    //        if (IsInsert)
    //        {
    //            // TempTransferObj tt = new TempTransferObj(HourId,ClassId,ct.TeacherId);
    //            // TransferObjList.Add(tt);
    //            break;
    //        }

    //        if (ct == EmptyHour.TeacherOptional.Last())
    //        {
    //            // var RandomCT = EmptyHour.TeacherOptional.Random();

    //            // IsInsert = ReplaceInsert(EmptyHour, ClassId, HourId, RandomCT.TeacherId, true);
    //            EmptyHour.SelectedTeacherIds.Add(-1);


    //        }



    //    }


    //    //if (!IsInsert)
    //    //{
    //    //    var RandomCT = EmptyHour.TeacherOptional.Random();
    //    //    //if (ExistTransfer(ClassId, HourId, RandomCT.TeacherId))
    //    //    //{
    //    //      //  RandomCT = EmptyHour.TeacherOptional.Random();
    //    //    //}
    //    //    IsInsert = ReplaceInsert(EmptyHour, ClassId, HourId, RandomCT.TeacherId, true);
    //    //   // TempTransferObj tt = new TempTransferObj(HourId, ClassId, RandomCT.TeacherId);
    //    //    //TransferObjList.Add(tt);
    //    //}


    //}




    private bool ExistTransfer(int classId, int hourId, int teacherId)
    {
        var Transfer = TransferObjList.FindAll(x => x.ClassId == classId && x.HourId == hourId && x.TeacherId == teacherId).FirstOrDefault();
        if (Transfer != null)
        {
            return true;

        }

        return false;
    }

    private bool ReplaceInsert(HourSchool emptyHour, int classId, int hourId, int teacherId, bool IsRandom, int Hakbatza, int Ihud)
    {

        var IsHakbatza = School.FindAll(x => x.HourId == hourId && x.SelectedTeacherIds.Count > 1
                                                   && x.SelectedTeacherIds.Contains(teacherId)).FirstOrDefault();
        // לא ליגוע בנעצים שנעצו לפני כן
        var IsNaatz = School.FindAll(x => x.TeacherNaatz.Any()).FirstOrDefault();

        if (IsHakbatza != null || IsNaatz != null) return false;


        // בשביל פיתוח
        //if (Hakbatza != 0 || Ihud != 0) return false;



        // מציאת שעות שהמורה שהוא אחד מהשעה החסרה
        var OptionalHoursFORTeacher = School.FindAll(x => x.ClassId == classId && x.SelectedTeacherIds.Count == 1
                                                     && x.SelectedTeacherIds.Contains(teacherId));


        foreach (HourSchool item in OptionalHoursFORTeacher)
        {



            // מציאת מורה שיכול להכנס לשעה הזו וחסר לו שעות בכיתה
            int SelectedTeacherId = SearchTeacherWithoutByClass(item.ClassId, item.HourId);


            // האם המורה נמצא באותה שעה ללמד
            //  var IsTeacherWorkInHour = School.FindAll(x => x.HourId == hourId && x.TeacherOptional.Any(y => y.TeacherId == teacherId));


            if (SelectedTeacherId != 0)
            {


                //בודק אם מורה מלמד שעתיים ברצף...
                //  if (!CheckValidation(item, SelectedTeacherId, emptyHour, teacherId)) return false;


                // מוחק את המורה שעובד בשעה הזו

                DeleteChooseTeacherFromHour(teacherId, item.HourId);
                //UpdateSchoolObject(item, SelectedTeacherId, item.Day, item.Hour, item.ClassId, 0, 0);





                // מוחק את המורה מהשעה החסרה אם הוא מלמד בכיתה אחרת
                //  DeleteChooseTeacherFromHour(teacherId, hourId);
                //מכניס אותו לשעה הפנויה
                UpdateSchoolObject(emptyHour, teacherId, emptyHour.Day, emptyHour.Hour, classId, 0, 0);



                return true;


            }
        }





        return false;

    }

    private bool CheckValidation(HourSchool item, int selectedTeacherId, HourSchool emptyHour, int teacherId)
    {
        var ctObj = item.TeacherOptional.FindAll(y => y.TeacherId == selectedTeacherId).FirstOrDefault();
        int ClassId = item.ClassId;
        int Day = item.Day;
        var TeacherInDayList = School.FindAll(x => x.ClassId == ClassId && x.Day == Day && x.TeacherOptional.Any(y => y.TeacherId == ctObj.TeacherId) && x.SelectedTeacherIds.Contains(ctObj.TeacherId));
        int TeacherInDay = TeacherInDayList.Count;
        var hsObj = item;
        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
            {
                ctObj = emptyHour.TeacherOptional.FindAll(y => y.TeacherId == teacherId).FirstOrDefault();
                ClassId = emptyHour.ClassId;
                Day = emptyHour.Day;
                TeacherInDayList = School.FindAll(x => x.ClassId == ClassId && x.Day == Day && x.TeacherOptional.Any(y => y.TeacherId == ctObj.TeacherId) && x.SelectedTeacherIds.Contains(ctObj.TeacherId));
                TeacherInDay = TeacherInDayList.Count;
                hsObj = emptyHour;

            }


            // בדיקה האם קיימת הגבלה לשעות ביום למורה
            var HourExtraObj = Config.HourExtraList.FindAll(x => x.ClassId == ClassId && x.DayId == Day && x.TeacherId == ctObj.TeacherId).FirstOrDefault();
            if (!ctObj.IsTeacher && TeacherInDay == Config.MaxHourInShibutz || (HourExtraObj != null && TeacherInDay == HourExtraObj.HourExtraHour))
            {

                return false;

            }

            // אם הוא מלמד רק שעתיים אל תתן באותו יום
            if (!ctObj.IsTwoHour && ctObj.HardTeacherHoursInClass <= Config.MinForPitzul && TeacherInDay + 1 == Config.MinForPitzul)
            {
                return false;

            }

            if (ctObj.IsTeacher && TeacherInDay > 4)
            {
                return false;
            }

            //// אם מורה הוא מורה מקצועי בכיתה אחרת אל תכניס אותו במקום הכיתה שלו 
            //var FirstHourForTeacher = School.FindAll(x => x.HourId == hsObj.HourId && x.ClassId != ClassId && x.Hour == 1 && x.TeacherOptional.Any(y => y.TeacherId == ctObj.TeacherId && y.IsTeacher)).FirstOrDefault();
            //if (FirstHourForTeacher != null)
            //{
            //    continue;
            //}

        }








        return true;
    }







    //private bool CheckifInsert(HourSchool emptyHour, int classId, int hourId, int teacherId)
    //{
    //    // בדיקהה אם חסרות שעות למורה בכיתה
    //    int TeacherIdInClassWithoutHour = SearchTeacherWithoutByClass(classId, hourId);//School.FindAll(x => x.ClassId==classId && x.TeacherOptional.Any(y => y.TeacherHoursInClass > 0 && y.TeacherId==teacherId)).FirstOrDefault();

    //    if (TeacherIdInClassWithoutHour != 0)
    //    {
    //        UpdateSchoolObject(emptyHour, teacherId, emptyHour.Day, emptyHour.Hour, classId);
    //        return true;
    //    }

    //    return false;



    //}

    //
    private int SearchTeacherWithoutByClass(int classId, int hourId)
    {

        // מציאת מורים באותה כיתה שצריכים ללמד ואין להם שעה
        var TeacherWithoutClass = School.SelectMany(x => x.TeacherOptional).Where(y => y.ClassId == classId && y.TeacherHoursInClass > 0);


        foreach (ClassTeacher item in TeacherWithoutClass)
        {
            // האם המורה עובד בכיתה כלשהיא באותה שעה בבית הספר בכלל
            var IsTeacherWork = School.FindAll(x => x.HourId == hourId && x.SelectedTeacherIds.Contains(item.TeacherId));
            // האם המורה נמצא באותה שעה ללמד
            var IsTeacherWorkInHour = School.FindAll(x => x.HourId == hourId && x.TeacherOptional.Any(y => y.TeacherId == item.TeacherId));


            if (IsTeacherWork.Count == 0 && IsTeacherWorkInHour.Count > 0)
            {
                return item.TeacherId;

            }


        }


        return 0;

    }



    //private void UpdateTeacherWithoutClass()
    //{

    //    var HourWithoutTeacher = School.FindAll(x => !x.SelectedTeacherIds.Any());


    //    var TeacherWithoutClass = School.FindAll(x => x.TeacherOptional.Any(y => y.TeacherHoursInClass > 0));
    //    foreach (HourSchool item in TeacherWithoutClass)
    //    {
    //        int ClassId = item.ClassId;
    //        var ctsObj = item.TeacherOptional.FindAll(x => x.TeacherHoursInClass > 0);


    //        foreach (ClassTeacher ct in ctsObj)
    //        {

    //            int TeacherId = ct.TeacherId;
    //            int TeacherHoursInClass = ct.TeacherHoursInClass;

    //            for (int i = 0; i < TeacherHoursInClass; i++)
    //            {
    //                //שעות ללא מורה בכיתה המדוברת
    //                var EmptyHour = School.FindAll(x => x.ClassId == ClassId && x.SelectedTeacherIds.Count == 0).FirstOrDefault();

    //                var TeacherWorkInClass = School.FindAll(x => x.ClassId == ClassId && x.SelectedTeacherIds.Count == 1 &&
    //                                         !x.SelectedTeacherIds.Contains(TeacherId));

    //                var TeacherFreeInClass = School.FindAll(x => x.ClassId == ClassId && x.TeacherOptional.Any(y => y.TeacherId == TeacherId && y.TeacherHoursInClass > 0));

    //                bool IsExist = false;

    //                if (EmptyHour == null)
    //                {

    //                    break;
    //                }



    //                foreach (HourSchool work in TeacherWorkInClass)
    //                {


    //                    foreach (ClassTeacher c in work.TeacherOptional)
    //                    {
    //                        var TeachEqual = EmptyHour.TeacherOptional.FindAll(x => x.TeacherId == c.TeacherId).FirstOrDefault();

    //                        if (TeachEqual != null)
    //                        {

    //                            if (TeacherFreeInClass.FindAll(x => x.HourId == work.HourId).Count > 0)
    //                            {

    //                                work.SelectedTeacherIds.Remove(work.SelectedTeacherIds[0]);
    //                                //  work.AddSelectedTeacher(TeacherId);
    //                                UpdateSchoolObject(work, TeacherId, work.Day, work.Hour, ClassId);

    //                                UpdateSchoolObject(EmptyHour, c.TeacherId, EmptyHour.Day, EmptyHour.Hour, ClassId);

    //                                IsExist = true;
    //                                break;


    //                            }

    //                        }

    //                    }

    //                    if (IsExist) break;

    //                }

    //            }
    //        }
    //    }

    //}

    private bool SelectTeacherInMainHakbatzaIhud(int ClassId, int Day, int Hour, TempTeachersObj ObjTeacher, HourSchool hsObj, List<TempTeachersObj> TempTeachers)
    {

        int Hakbatza = ObjTeacher.ctObj.Hakbatza;

        int CountInHakbatza = GetHakbatzaIhudCount(1, Hakbatza);
        var HakbatzaList = TempTeachers.FindAll(x => x.ctObj.Hakbatza == Hakbatza);

        int Ihud = ObjTeacher.ctObj.Ihud;
        int IhudTeacherId = ObjTeacher.TeacherId;
        int CountInIhud = GetHakbatzaIhudCount(2, Ihud);
        var IhudList = School.FindAll(u => u.Day == Day && u.Hour == Hour && !u.SelectedTeacherIds.Any()
                                    && u.TeacherOptional.Any(y => y.Ihud == Ihud && y.TeacherId == IhudTeacherId && y.TeacherHoursInClass > 0));



        // אם יש רק הקבצה
        if (CountInHakbatza == HakbatzaList.Count && Hakbatza != 0)
        {

            Hashtable TeachersIhud = new Hashtable();

            foreach (TempTeachersObj item in HakbatzaList)
            {
                if (Ihud != 0)
                {

                    IhudList = School.FindAll(u => u.Day == Day && u.Hour == Hour && !u.SelectedTeacherIds.Any()
                                    && u.TeacherOptional.Any(y => y.Ihud == Ihud && y.TeacherId == item.TeacherId && y.TeacherHoursInClass > 0));

                    if (IhudList.Count != CountInIhud / HakbatzaList.Count)
                    {
                        return false;

                    }


                    TeachersIhud.Add(item.TeacherId, IhudList);


                }
                else
                {
                    UpdateSchoolObject(hsObj, item.TeacherId, Day, Hour, ClassId, item.ctObj.Hakbatza, item.ctObj.Ihud);

                }

            }

            SetHakbatzaIhudCount(1, Hakbatza);

            if (Ihud != 0)
            {
                foreach (DictionaryEntry entry in TeachersIhud)
                {
                    InsertToIhud((List<HourSchool>)entry.Value, (int)entry.Key, Hour, Day, Hakbatza, Ihud);
                }

                SetHakbatzaIhudCount(2, Ihud);

            }


            return true;

        }


        // אם יש רק איחוד
        if (CountInIhud == IhudList.Count && Hakbatza == 0 && Ihud != 0)
        {
            InsertToIhud(IhudList, IhudTeacherId, Hour, Day, Hakbatza, Ihud);
            SetHakbatzaIhudCount(2, Ihud);

            return true;

        }

        return false;



    }

    private void SelectTeacherInMainRegular(int ClassId, int Day, int Hour, TempTeachersObj ObjTeacher, HourSchool hsObj)
    {

        // לבדוק אפשרות של הצמדה
        //if (ObjTeacher.ctObj.IsTwoHour && ObjTeacher.TeacherInDay == 0)
        //{
        //    var TeachList = School.FindAll(u => u.Day == Day && u.Hour == Hour + 1 && !u.SelectedTeacherIds.Any()
        //                             && u.TeacherOptional.Any(y => y.TeacherId == ObjTeacher.TeacherId && y.TeacherHoursInClass > 0)).FirstOrDefault();

        //    if (TeachList != null)
        //    {


        //        UpdateSchoolObject(TeachList, ObjTeacher.TeacherId, Day, Hour + 1, ClassId);

        //    }
        //}

        UpdateSchoolObject(hsObj, ObjTeacher.TeacherId, Day, Hour, ClassId, ObjTeacher.ctObj.Hakbatza, ObjTeacher.ctObj.Ihud);

    }

    private void UpdateSchoolObject(HourSchool hsObj, int TeacherId, int Day, int Hour, int ClassId, int Hakbatza, int Ihud)
    {
        // עדכון והוספה



        var TeacherStatusInClass = School.FindAll(u => u.ClassId == ClassId).SelectMany(x => x.TeacherOptional).Where(y => y.TeacherId == TeacherId && y.Hakbatza == Hakbatza && y.Ihud == Ihud && y.TeacherHoursInClass > 0).FirstOrDefault();

        if (TeacherStatusInClass == null)
        {
            return;
        }




        hsObj.AddSelectedTeacher(TeacherId);
        hsObj.SelectedHakbatza = Hakbatza;
        hsObj.SelectedIhud = Ihud;










        // עדכון כל השעות של המורה בכיתה להוריד 1
        School.FindAll(u => u.ClassId == ClassId).SelectMany(x => x.TeacherOptional).Where(y => y.TeacherId == TeacherId && y.Hakbatza == Hakbatza && y.Ihud == Ihud).ToList().ForEach(c =>
        {
            if (c.TeacherHoursInClass > 0)
            {
                c.LastTeacherHoursInClass = c.TeacherHoursInClass;
                c.TeacherHoursInClass -= 1;

            }
            else
            {

                if (c.LastTeacherHoursInClass > 0) c.LastTeacherHoursInClass -= 1;

            }

        });


        School.FindAll(u => u.Hour == Hour && u.Day == Day).SelectMany(x => x.TeacherOptional).Where(y => y.TeacherId == TeacherId).ToList().ForEach(c =>
        {
            if (c.TeacherHoursInClass > 0)
            {
                c.LastTeacherHoursInClass = (c.ClassId == ClassId) ? c.LastTeacherHoursInClass : c.TeacherHoursInClass;

            }
            else
            {

                if (c.LastTeacherHoursInClass > 0) c.LastTeacherHoursInClass -= 1;
            }

            c.TeacherHoursInClass = 0;

        });


      








    }


    private bool DeleteChooseTeacherFromHour(int teacherId, int hourId)
    {
        // מחיקה של בחירה
        var SelectedInAnother = School.FindAll(x => x.HourId == hourId && x.SelectedTeacherIds.Count == 1 && x.SelectedTeacherIds.Contains(teacherId)).FirstOrDefault();
        if (SelectedInAnother != null)
        {

            SelectedInAnother.SelectedTeacherIds.Remove(SelectedInAnother.SelectedTeacherIds[0]);


            // עדכון כל השעות של המורה בכיתה להוסיף 1
            School.FindAll(u => u.ClassId == SelectedInAnother.ClassId).SelectMany(x => x.TeacherOptional).Where(y => y.TeacherId == teacherId && y.Hakbatza == 0 && y.Ihud == 0).ToList().ForEach(c =>
            {

                //  c.TeacherHoursInClass += 1;
                // Do some stuff here.

                //c.TeacherHoursInClass += 1;
                //c.LastTeacherHoursInClass += 1;
                c.TeacherHoursInClass = c.LastTeacherHoursInClass;
            });


            School.FindAll(u => u.HourId == hourId).SelectMany(x => x.TeacherOptional).Where(y => y.TeacherId == teacherId).ToList().ForEach(c =>
            {
                {

                    c.TeacherHoursInClass = c.LastTeacherHoursInClass;

                }
            });


            //if (teacherId == 96)
            //{
            //    var temp = School.FindAll(u => u.ClassId == 2060 && u.TeacherOptional.Any(y => y.TeacherId == teacherId)).ToList();
            //    var temp2 = School.FindAll(u => u.ClassId == 2061 && u.TeacherOptional.Any(y => y.TeacherId == teacherId)).ToList();
            //    var temp3 = School.FindAll(u => u.ClassId == 2062 && u.TeacherOptional.Any(y => y.TeacherId == teacherId)).ToList();
            //}




            return true;
        }





        return false;
    }

    private int GetHakbatzaIhudCount(int Type, int Id)
    {

        var HakbatzaIhud = HakbatzaIhudList.FindAll(x => x.Type == Type && x.Id == Id).FirstOrDefault();

        if (HakbatzaIhud != null)
        {

            return HakbatzaIhud.Counter;
        }

        return 0;

    }

    private void SetHakbatzaIhudCount(int Type, int Id)
    {

        var HakbatzaIhud = HakbatzaIhudList.FindAll(x => x.Type == Type && x.Id == Id).FirstOrDefault();

        if (HakbatzaIhud != null)
        {


            HakbatzaIhudList.FindAll(x => x.Type == Type && x.Id == Id).ToList().ForEach(c =>
            {
                c.Hour -= 1;
            });



        }


    }

    private bool IsHakIhudSet(int Type, int Id)
    {

        var HakbatzaIhud = HakbatzaIhudList.FindAll(x => x.Type == Type && x.Id == Id).FirstOrDefault();

        if (HakbatzaIhud != null)
        {

            if (HakbatzaIhud.Hour == 0) return false;
        }

        return true;

    }

    private void InsertToIhud(List<HourSchool> IhudList, int IhudTeacherId, int Hour, int Day, int Hakbatza, int Ihud)
    {
        foreach (HourSchool item in IhudList)
        {


            UpdateSchoolObject(item, IhudTeacherId, Day, Hour, item.ClassId, Hakbatza, Ihud);

            //item.AddSelectedTeacher(IhudTeacherId);

            ////עדכון כל השעות של המורה בכיתה להוריד 1
            //School.FindAll(u => u.ClassId == item.ClassId).SelectMany(x => x.TeacherOptional).Where(y => y.TeacherId == IhudTeacherId).ToList().ForEach(c =>
            //{
            //    if (c.TeacherHoursInClass > 0)
            //    {
            //        c.TeacherHoursInClass -= 1;

            //    }
            //});

            //School.FindAll(u => u.Hour == Hour && u.Day == Day).SelectMany(x => x.TeacherOptional).Where(y => y.TeacherId == IhudTeacherId).ToList().ForEach(c => c.TeacherHoursInClass = 0);


        }
    }

    private void GetTeacherStatus(ref int teacherFree, ref int teacherHourWork, ref int TeacherInDay, int TeacherId, int ClassId, int Day, int Hour)
    {

        // חיפוש כל השעות הפנויות של המורה בכיתה והם ריקות ולא משובץ 
        // הוספתי תנאי שאמנם השעות ריקות אבל שהם לא תפוסות בכיתה אחרת וכך אני מקבל את השעות הפנויות האמיתיות
        var TeacherFreeList = School.FindAll(x => x.ClassId == ClassId && x.TeacherOptional.Any(y => y.TeacherId == TeacherId && y.TeacherHoursInClass > 0) && !x.SelectedTeacherIds.Any());//.SelectMany(y => y.TeacherOptional.);  // && x.TeacherOptional. && !x.SelectedTeacherIds.Any());

        // המורה נמצא בשעה הספציפית ביום הזה האלו 
        var TeacherHourWorkList = School.FindAll(x => x.Day == Day && x.Hour == Hour && x.TeacherOptional.Any(y => y.TeacherId == TeacherId));

        int SumHourForTeacher = 0;
        List<int> InnerClassId = new List<int>();
        foreach (HourSchool item in TeacherHourWorkList)
        {
            if (!InnerClassId.Contains(item.ClassId))
            {
                // סופר את כל השעות שהוא יכול ללמד שם
                SumHourForTeacher += item.TeacherOptional.FirstOrDefault(x => x.TeacherId == TeacherId).TeacherHoursInClass;
                InnerClassId.Add(item.ClassId);

            }


        }

        // המורה משובץ לכיתה זו בתוך היום הזה
        var TeacherInDayList = School.FindAll(x => x.ClassId == ClassId && x.Day == Day && x.TeacherOptional.Any(y => y.TeacherId == TeacherId) && x.SelectedTeacherIds.Contains(TeacherId));



        teacherFree = TeacherFreeList.Count();
        teacherHourWork = SumHourForTeacher;
        TeacherInDay = TeacherInDayList.Count();

        //// מציאת מורה שזו השעה האחרונה שהוא יכול להשתבץ
        //var ObjLast = TempTeachers.FindAll(x => x.TeacherFree == 1).FirstOrDefault();

        //// מציאת מורה שמה שנשאר לו שווה למה שהוא עוד צריך
        //var ObjLastEmpty = TempTeachers.FindAll(x => x.TeacherHourWork - x.TeacherFree == 0).FirstOrDefault();


    }
}

public class ClassTeacher
{
    public ClassTeacher(string item, int ClassId)
    {

        string[] Teacher = item.Split('-');
        this.TeacherId = Helper.ConvertToInt(Teacher[0]);
        this.TeacherHoursInClass = Helper.ConvertToInt(Teacher[1]);
        this.HardTeacherHoursInClass = Helper.ConvertToInt(Teacher[1]);
        this.LastTeacherHoursInClass = Helper.ConvertToInt(Teacher[1]);
        this.IsTeacher = (Teacher[2] == "0") ? false : true;
        this.Hakbatza = Helper.ConvertToInt(Teacher[3]);
        this.Ihud = Helper.ConvertToInt(Teacher[4]);
        this.ProfessionalId = Helper.ConvertToInt(Teacher[5]);
        this.TafkidId = Helper.ConvertToInt(Teacher[6]);
        this.IsTwoHour = (Teacher[7] == "0") ? false : true;
        this.ClassId = ClassId;


    }

    public int ClassId;
    public int TeacherId;
    public int LastTeacherHoursInClass;
    public int TeacherHoursInClass;
    public int HardTeacherHoursInClass;
    public bool IsTeacher;
    public int Hakbatza;
    public int Ihud;
    public int ProfessionalId;
    public int TafkidId;

    public bool IsTwoHour;


}


public class TempTransferObj
{
    public TempTransferObj(int HourId, int ClassId, int TeacherId)
    {
        this.HourId = HourId;
        this.ClassId = ClassId;
        this.TeacherId = TeacherId;
    }
    public int HourId;
    public int ClassId;
    public int TeacherId;
}
public class HourSchool
{
    public HourSchool(DataRow row)
    {
        this.ClassId = Helper.ConvertToInt(row["ClassId"].ToString());
        this.HourId = Helper.ConvertToInt(row["HourId"].ToString());
        this.Day = Helper.ConvertToInt(row["HourId"].ToString().Substring(0, 1));
        this.Hour = Helper.ConvertToInt(row["HourId"].ToString().Substring(1, 1));
        this.TeacherNaatz = row["TeacherNaatz"].ToString();

        this.TeacherOptional = new List<ClassTeacher>();
        this.SelectedTeacherIds = new List<int>();

    }

    public int HourId;
    public int Day;
    public int Hour;
    public int ClassId;
    public string TeacherNaatz;
    public int SelectedHakbatza;
    public int SelectedIhud;


    public List<ClassTeacher> TeacherOptional;
    public List<int> SelectedTeacherIds;
    public bool IsHasHakbatza;
    public bool IsHasIhud;


    public void AddClassTeacher(ClassTeacher ct)
    {

        this.TeacherOptional.Add(ct);

    }

    public void AddSelectedTeacher(int SelectedTeacherId)
    {

        this.SelectedTeacherIds.Add(SelectedTeacherId);


    }


}



public class Configuration
{
    public int MaxHourInShibutz;
    public int MinForPitzul;
    public int MinTeacherInmor;
    public int ConfigurationId;

    public List<HourExtra> HourExtraList;

    public Configuration(DataRow row)
    {
        this.MaxHourInShibutz = Helper.ConvertToInt(row["MaxHourInShibutz"].ToString());
        this.MinForPitzul = Helper.ConvertToInt(row["MinForPitzul"].ToString());
        this.MinTeacherInmor = Helper.ConvertToInt(row["MinTeacherInmor"].ToString());
        this.HourExtraList = new List<HourExtra>();
        this.ConfigurationId = Helper.ConvertToInt(row["ConfigurationId"].ToString());
    }


    public void AddHourExtra(HourExtra he)
    {

        this.HourExtraList.Add(he);

    }
}


public class TempTeachersObj
{
    public int TeacherId;
    public int ClassId;
    public int TeacherFree;
    public int TeacherHourWork;
    public int TeacherInDay;
    public bool IsOk;

    public ClassTeacher ctObj;
    public TempTeachersObj() { }

}



public class LastSelectedTeacherObj
{
    public int TeacherId;
    public int ClassId;
    public int Hour;
    public int Day;


    public LastSelectedTeacherObj() { }

}





public class HourExtra
{
    public HourExtra(DataRow row)
    {
        this.TeacherId = Helper.ConvertToInt(row["TeacherId"].ToString());
        this.ClassId = Helper.ConvertToInt(row["ClassId"].ToString());
        this.DayId = Helper.ConvertToInt(row["DayId"].ToString());
        this.HourExtraHour = Helper.ConvertToInt(row["HourExtra"].ToString());
    }


    public int TeacherId;
    public int ClassId;
    public int DayId;
    public int HourExtraHour;

}



public class HakbatzaIhud
{
    public HakbatzaIhud(DataRow row)
    {
        this.Type = Helper.ConvertToInt(row["Type"].ToString());
        this.Counter = Helper.ConvertToInt(row["Counter"].ToString());
        this.ClassId = Helper.ConvertToInt(row["ClassId"].ToString());
        this.Id = Helper.ConvertToInt(row["Id"].ToString());
        this.Hour = Helper.ConvertToInt(row["Hour"].ToString());
        this.CounterSet = 0;
    }

    public int Type;
    public int Counter;
    public int ClassId;
    public int Id;
    public int CounterSet;
    public int Hour;

}






public static class Helper
{

    public static int ConvertToInt(string val)
    {

        int res;
        bool isOk = Int32.TryParse(val, out res);

        if (isOk)
            return res;

        return 0;


    }

}



public static class EnumerableHelper<E>
{
    private static Random r;

    static EnumerableHelper()
    {
        r = new Random();
    }

    public static T Random<T>(IEnumerable<T> input)
    {
        return input.ElementAt(r.Next(input.Count()));
    }

}

public static class EnumerableExtensions
{
    public static T Random<T>(this IEnumerable<T> input)
    {
        return EnumerableHelper<T>.Random(input);
    }
}
