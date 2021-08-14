
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace AowCore.AppWeb.Helpers
{
    public class SelectListItemsDropdown
    {
        public List<SelectListItem> getVoucherList()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="1",Text="Sales"},
                 new SelectListItem{ Value="2",Text="Expense"},
                 new SelectListItem{ Value="3",Text="Revenue"},
                  new SelectListItem{ Value="4",Text="Purchase"},
                   new SelectListItem{ Value="5",Text="Payment"},
                    new SelectListItem{ Value="6",Text="Sales Recipt"}
             };
            myList = data.ToList();
            return myList;
        }


        public List<SelectListItem> getItemTypeList()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="1",Text="Inventory Item"},
                 new SelectListItem{ Value="2",Text="Non-Inventory Item"},
                  new SelectListItem{ Value="3",Text="Service"},
                   new SelectListItem{ Value="4",Text="Discount"},
                    new SelectListItem{ Value="5",Text="Taxation"},
                    new SelectListItem{ Value="6",Text="PayMent Received"},

             };
            myList = data.ToList();
            return myList;
        }

        public List<SelectListItem> getIndustryTypeList()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="1",Text="Manufacturing"},
                 new SelectListItem{ Value="2",Text="Retail"},
                  new SelectListItem{ Value="3",Text="Food Services"},
                   new SelectListItem{ Value="4",Text="Education"},
                    new SelectListItem{ Value="5",Text="Automotive"},
                    new SelectListItem{ Value="6",Text="Financial Services"},
                     new SelectListItem{ Value="7",Text="Transport"},

             };
            myList = data.ToList();
            return myList;
        }
        public List<SelectListItem> getDoMyAccountingList()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="1",Text="Pen & Paper"},
                 new SelectListItem{ Value="2",Text="Excel"},
                  new SelectListItem{ Value="3",Text="Tally"},
                   new SelectListItem{ Value="4",Text="Quick Books"},
                    new SelectListItem{ Value="5",Text="Sage"},
                    new SelectListItem{ Value="6",Text="Zoho"},
                     new SelectListItem{ Value="7",Text="SAP"},
                     new SelectListItem{ Value="8",Text="Other"},

             };
            myList = data.ToList();
            return myList;
        }


        public List<SelectListItem> getBussinessTypeList()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="1",Text="Sole ProperitorShip"},
                 new SelectListItem{ Value="2",Text="PartnerShip"},
                  new SelectListItem{ Value="3",Text="LLC"},
                   new SelectListItem{ Value="4",Text="Corporation"},
                    new SelectListItem{ Value="5",Text="Automotive"},
                    new SelectListItem{ Value="6",Text="Non Registered"},
                     new SelectListItem{ Value="7",Text="Transport"},

             };
            myList = data.ToList();
            return myList;
        }


        public static Dictionary<string, string> Currencies = new Dictionary<string, string>() {
                                                    {"AED", "د.إ.‏"},
                                                    {"AFN", "؋ "},
                                                    {"ALL", "Lek"},
                                                    {"AMD", "դր."},
                                                    {"ARS", "$"},
                                                    {"AUD", "$"},
                                                    {"AZN", "man."},
                                                    {"BAM", "KM"},
                                                    {"BDT", "৳"},
                                                    {"BGN", "лв."},
                                                    {"BHD", "د.ب.‏ "},
                                                    {"BND", "$"},
                                                    {"BOB", "$b"},
                                                    {"BRL", "R$"},
                                                    {"BYR", "р."},
                                                    {"BZD", "BZ$"},
                                                    {"CAD", "$"},
                                                    {"CHF", "fr."},
                                                    {"CLP", "$"},
                                                    {"CNY", "¥"},
                                                    {"COP", "$"},
                                                    {"CRC", "₡"},
                                                    {"CSD", "Din."},
                                                    {"CZK", "Kč"},
                                                    {"DKK", "kr."},
                                                    {"DOP", "RD$"},
                                                    {"DZD", "DZD"},
                                                    {"EEK", "kr"},
                                                    {"EGP", "ج.م.‏ "},
                                                    {"ETB", "ETB"},
                                                    {"EUR", "€"},
                                                    {"GBP", "£"},
                                                    {"GEL", "Lari"},
                                                    {"GTQ", "Q"},
                                                    {"HKD", "HK$"},
                                                    {"HNL", "L."},
                                                    {"HRK", "kn"},
                                                    {"HUF", "Ft"},
                                                    {"IDR", "Rp"},
                                                    {"ILS", "₪"},
                                                    {"INR", "रु"},
                                                    {"IQD", "د.ع.‏ "},
                                                    {"IRR", "ريال "},
                                                    {"ISK", "kr."},
                                                    {"JMD", "J$"},
                                                    {"JOD", "د.ا.‏ "},
                                                    {"JPY", "¥"},
                                                    {"KES", "S"},
                                                    {"KGS", "сом"},
                                                    {"KHR", "៛"},
                                                    {"KRW", "₩"},
                                                    {"KWD", "د.ك.‏ "},
                                                    {"KZT", "Т"},
                                                    {"LAK", "₭"},
                                                    {"LBP", "ل.ل.‏ "},
                                                    {"LKR", "රු."},
                                                    {"LTL", "Lt"},
                                                    {"LVL", "Ls"},
                                                    {"LYD", "د.ل.‏ "},
                                                    {"MAD", "د.م.‏ "},
                                                    {"MKD", "ден."},
                                                    {"MNT", "₮"},
                                                    {"MOP", "MOP"},
                                                    {"MVR", "ރ."},
                                                    {"MXN", "$"},
                                                    {"MYR", "RM"},
                                                    {"NIO", "N"},
                                                    {"NOK", "kr"},
                                                    {"NPR", "रु"},
                                                    {"NZD", "$"},
                                                    {"OMR", "ر.ع.‏ "},
                                                    {"PAB", "B/."},
                                                    {"PEN", "S/."},
                                                    {"PHP", "PhP"},
                                                    {"PKR", "Rs"},
                                                    {"PLN", "zł"},
                                                    {"PYG", "Gs"},
                                                    {"QAR", "ر.ق.‏ "},
                                                    {"RON", "lei"},
                                                    {"RSD", "Din."},
                                                    {"RUB", "р."},
                                                    {"RWF", "RWF"},
                                                    {"SAR", "ر.س.‏ "},
                                                    {"SEK", "kr"},
                                                    {"SGD", "$"},
                                                    {"SYP", "ل.س.‏ "},
                                                    {"THB", "฿"},
                                                    {"TJS", "т.р."},
                                                    {"TMT", "m."},
                                                    {"TND", "د.ت.‏ "},
                                                    {"TRY", "TL"},
                                                    {"TTD", "TT$"},
                                                    {"TWD", "NT$"},
                                                    {"UAH", "₴"},
                                                    {"USD", "$"},
                                                    {"UYU", "$U"},
                                                    {"UZS", "so'm"},
                                                    {"VEF", "Bs. F."},
                                                    {"VND", "₫"},
                                                    {"XOF", "XOF"},
                                                    {"YER", "ر.ي.‏ "},
                                                    {"ZAR", "R"},
                                                    {"ZWL", "Z$"} };




        public class Currency
        {
            public string ISOName { get; set; }
            public string CurrencyName { get; set; }
            public string Symbol { get; set; }



            protected void CuurencySeed()
            {
                var category = new List<Currency>
                        {
             new Currency {ISOName="AFN",   CurrencyName = "Afghanistan Afghani",Symbol=""},

        };
                // category.ForEach(s => context.AccountCategories.AddOrUpdate(s));
                //context.SaveChanges();
                List<Currency> myList = new List<Currency>();
                myList.Add(new Currency() { ISOName = "AFN", CurrencyName = "Afghanistan Afghani", Symbol = "د.إ." });
            }

        }

        public List<SelectListItem> getCurrencyList()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{

                   new SelectListItem{ Value="AFN",Text="Afghanistan Afghani"},
                     new SelectListItem{ Value="ALL",Text="Albanian Lek"},
                  new SelectListItem{ Value="DZD",Text="Algeria Dinars"},
                  new SelectListItem{ Value="ADP",Text="Andorran Peseta"},
                   new SelectListItem{ Value="AOK",Text="Angolan Kwanza"},
                   new SelectListItem{ Value="ARS",Text="Argentina Pesos"},
                    new SelectListItem{ Value="AMD",Text="Armenian Dram"},
                    new SelectListItem{ Value="AUD",Text="Australia Dollars"},
                     new SelectListItem{ Value="7",Text="Bahamas Dollars"},
                     new SelectListItem{ Value="7",Text="Barbados Dollars"},
                     new SelectListItem{ Value="7",Text="Belgium Francs"},
                     new SelectListItem{ Value="7",Text="Bermuda Dollars"},
                     new SelectListItem{ Value="7",Text="Brazil Real"},
                     new SelectListItem{ Value="7",Text="Bulgaria Lev"},
                     new SelectListItem{ Value="7",Text="Canada Dollars"},
                     new SelectListItem{ Value="7",Text="Chile Pesos"},
                     new SelectListItem{ Value="7",Text="China Yuan Renmimbi"},
                     new SelectListItem{ Value="7",Text="Cyprus Pounds"},
                     new SelectListItem{ Value="7",Text="Czech Republic Koruna"},
                     new SelectListItem{ Value="7",Text="Denmark Kroner"},
                     new SelectListItem{ Value="7",Text="Dutch Guilders"},
                      new SelectListItem{ Value="7",Text="Eastern Caribbean Dollars"},
                     new SelectListItem{ Value="7",Text="Egypt Pounds"},
                      new SelectListItem{ Value="EUR",Text="Euro"},
                     new SelectListItem{ Value="7",Text="Fiji Dollars"},
                     new SelectListItem{ Value="7",Text="Finland Markka"},
                     new SelectListItem{ Value="7",Text="France Francs"},
                     new SelectListItem{ Value="7",Text="Germany Deutsche Marks"},
                     new SelectListItem{ Value="7",Text="Gold Ounces"},
                     new SelectListItem{ Value="7",Text="Greece Drachmas"},
                     new SelectListItem{ Value="7",Text="Hong Kong Dollars"},
                     new SelectListItem{ Value="7",Text="Hungary Forint"},
                     new SelectListItem{ Value="7",Text="Iceland Krona"},
                     new SelectListItem{ Value="INR",Text="India Rupees"},
                     new SelectListItem{ Value="7",Text="Indonesia Rupiah"},
                      new SelectListItem{ Value="7",Text="Ireland Punt"},
                       new SelectListItem{ Value="7",Text="Israel New Shekels"},
                        new SelectListItem{ Value="ILS",Text="Italy Lira"},
                            new SelectListItem{ Value="JMD",Text="Jamaica Dollars"},
                                new SelectListItem{ Value="JPY",Text="Japan Yen"},
                                    new SelectListItem{ Value="JOD",Text="Jordan Dinar"},
                                        new SelectListItem{ Value="KRW",Text="Korea(South) Won"},
                                            new SelectListItem{ Value="LBP",Text="Lebanon Pounds"},
                                                new SelectListItem{ Value="LUF",Text="Luxembourg Francs"},
                                                    new SelectListItem{ Value="MYR",Text="Malaysia Ringgit"},
                                                      new SelectListItem{ Value="MXP",Text="Mexico Pesos"},
                                    new SelectListItem{ Value="NLG",Text="Netherlands Guilders"},
                                        new SelectListItem{ Value="NZD",Text="New Zealand Dollars"},
                                            new SelectListItem{ Value="NOK",Text="Norway Kroner"},
                                            new SelectListItem{ Value="USD",Text="United States Dollars"},
                                             new SelectListItem{ Value="GBP",Text="United Kingdom Pounds"},

             };
            myList = data.ToList();
            return myList;
        }

        public string getCurrencySymbol(string CurrencyCode)
        {
            string symbol = string.Empty;
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            IList Result = new ArrayList();
            foreach (CultureInfo ci in cultures)
            {
                RegionInfo ri = new RegionInfo(ci.Name);
                if (ri.ISOCurrencySymbol == CurrencyCode)
                {
                    symbol = ri.CurrencySymbol;
                    return symbol;
                }
            }
            return symbol;

        }


        //<option value = "USD" > United States Dollars</option>
        //                                   <option value = "EUR" > Euro </ option >

        //                                       < option value= "GBP" > United Kingdom Pounds</option>
        //                                   <option value = "DZD" > Algeria Dinars</option>
        //                                       <option value = "ARP" > Argentina Pesos</option>
        //                                       <option value = "AUD" > Australia Dollars</option>
        //                                       <option value = "ATS" > Austria Schillings</option>
        //                                       <option value = "BSD" > Bahamas Dollars</option>
        //                                       <option value = "BBD" > Barbados Dollars</option>
        //                                       <option value = "BEF" > Belgium Francs</option>
        //                                       <option value = "BMD" > Bermuda Dollars</option>
        //                                       <option value = "BRR" > Brazil Real</option>
        //                                       <option value = "BGL" > Bulgaria Lev</option>
        //                                       <option value = "CAD" > Canada Dollars</option>
        //                                       <option value = "CLP" > Chile Pesos</option>
        //                                       <option value = "CNY" > China Yuan Renmimbi</option>
        //                                   <option value = "CYP" > Cyprus Pounds</option>
        //                                       <option value = "CSK" > Czech Republic Koruna</option>
        //                                   <option value = "DKK" > Denmark Kroner</option>
        //                                       <option value = "NLG" > Dutch Guilders</option>
        //                                       <option value = "XCD" > Eastern Caribbean Dollars</option>
        //                                   <option value = "EGP" > Egypt Pounds</option>
        //                                       <option value = "FJD" > Fiji Dollars</option>
        //                                       <option value = "FIM" > Finland Markka</option>
        //                                       <option value = "FRF" > France Francs</option>
        //                                       <option value = "DEM" > Germany Deutsche Marks</option>
        //                                   <option value = "XAU" > Gold Ounces</option>
        //                                       <option value = "GRD" > Greece Drachmas</option>
        //                                       <option value = "HKD" > Hong Kong Dollars</option>
        //                                   <option value = "HUF" > Hungary Forint</option>
        //                                       <option value = "ISK" > Iceland Krona</option>
        //                                       <option value = "INR" > India Rupees</option>
        //                                       <option value = "IDR" > Indonesia Rupiah</option>
        //                                       <option value = "IEP" > Ireland Punt</option>
        //                                       <option value = "ILS" > Israel New Shekels</option>
        //                                   <option value = "ITL" > Italy Lira</option>
        //                                       <option value = "JMD" > Jamaica Dollars</option>
        //                                       <option value = "JPY" > Japan Yen</option>
        //                                       <option value = "JOD" > Jordan Dinar</option>
        //                                       <option value = "KRW" > Korea(South) Won</option>
        //                                       <option value = "LBP" > Lebanon Pounds</option>
        //                                       <option value = "LUF" > Luxembourg Francs</option>
        //                                       <option value = "MYR" > Malaysia Ringgit</option>
        //                                       <option value = "MXP" > Mexico Pesos</option>
        //                                       <option value = "NLG" > Netherlands Guilders</option>
        //                                       <option value = "NZD" > New Zealand Dollars</option>
        //                                   <option value = "NOK" > Norway Kroner</option>
        //                                       <option value = "PKR" > Pakistan Rupees</option>
        //                                       <option value = "XPD" > Palladium Ounces</option>
        //                                       <option value = "PHP" > Philippines Pesos</option>
        //                                       <option value = "XPT" > Platinum Ounces</option>
        //                                       <option value = "PLZ" > Poland Zloty</option>
        //                                       <option value = "PTE" > Portugal Escudo</option>
        //                                       <option value = "ROL" > Romania Leu</option>
        //                                       <option value = "RUR" > Russia Rubles</option>
        //                                       <option value = "SAR" > Saudi Arabia Riyal</option>
        //                                   <option value = "XAG" > Silver Ounces</option>
        //                                       <option value = "SGD" > Singapore Dollars</option>
        //                                       <option value = "SKK" > Slovakia Koruna</option>
        //                                       <option value = "ZAR" > South Africa Rand</option>
        //                                   <option value = "KRW" > South Korea Won</option>
        //                                   <option value = "ESP" > Spain Pesetas</option>
        //                                       <option value = "XDR" > Special Drawing Right (IMF)</option>
        //                                   <option value = "SDD" > Sudan Dinar</option>
        //                                   <option value = "SEK" > Sweden Krona</option>
        //                                   <option value = "CHF" > Switzerland Francs</option>
        //                                   <option value = "TWD" > Taiwan Dollars</option>
        //                                   <option value = "THB" > Thailand Baht</option>
        //                                   <option value = "TTD" > Trinidad and Tobago Dollars</option>
        //                                   <option value = "TRL" > Turkey Lira</option>
        //                                   <option value = "VEB" > Venezuela Bolivar</option>
        //                                   <option value = "ZMK" > Zambia Kwacha</option>
        //                                   <option value = "EUR" > Euro </ option >
        //                                   < option value="XCD">Eastern Caribbean Dollars</option>
        //                                   <option value = "XDR" > Special Drawing Right(IMF)</option>
        //                                   <option value = "XAG" > Silver Ounces</option>
        //                                   <option value = "XAU" > Gold Ounces</option>
        //                                   <option value = "XPD" > Palladium Ounces</option>
        //                                   <option value = "XPT" > Platinum Ounces</option>

        public List<SelectListItem> getTimeZoneList()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="-12",Text="(GMT - 12:00) Eniwetok, Kwajalein"},
                 new SelectListItem{ Value="-11",Text="(GMT - 10:00) Hawaii"},
                  new SelectListItem{ Value="-10",Text="(GMT - 11:00) Midway Island, Samoa"},
                  new SelectListItem{ Value="-9",Text="(GMT - 9:00) Alaska"},
                   new SelectListItem{ Value="-8",Text="(GMT - 8:00) Pacific Time(US &amp; Canada)"},
                    new SelectListItem{ Value="-7",Text="(GMT - 7:00) Mountain Time(US &amp; Canada)"},
                    new SelectListItem{ Value="-6",Text="(GMT - 6:00) Central Time(US &amp; Canada), Mexico City"},
                     new SelectListItem{ Value="-5",Text="(GMT - 5:00) Eastern Time(US &amp; Canada), Bogota, Lima"},
                     new SelectListItem{ Value="-4.5",Text="(GMT - 4:30) Caracas"},
                     new SelectListItem{ Value="-4",Text="(GMT - 4:00) Atlantic Time(Canada), La Paz, Santiago"},
                     new SelectListItem{ Value="-3.5",Text="(GMT - 3:30) Newfoundland"},
                     new SelectListItem{ Value="-3",Text="(GMT - 3:00) Brazil, Buenos Aires, Georgetown"},
                     new SelectListItem{ Value="-2",Text="(GMT - 2:00) Mid-Atlantic"},
                     new SelectListItem{ Value="1",Text="(GMT - 1:00 hour) Azores, Cape Verde Islands"},
                     new SelectListItem{ Value="0",Text="(GMT)Western Europe Time, London, Lisbon, Casablanca, Greenwich"},
                     new SelectListItem{ Value="1",Text="(GMT + 1:00 hour) Brussels, Copenhagen, Madrid, Paris"},
                     new SelectListItem{ Value="2",Text="(GMT + 2:00) Kaliningrad, South Africa, Cairo"},
                     new SelectListItem{ Value="3",Text="(GMT + 3:00) Baghdad, Riyadh, Moscow, St.Petersburg"},
                     new SelectListItem{ Value="3.5",Text="(GMT + 3:30) Tehran"},
                     new SelectListItem{ Value="4",Text="(GMT + 4:00) Abu Dhabi, Muscat, Yerevan, Baku, Tbilisi"},
                     new SelectListItem{ Value="4.5",Text="(GMT + 4:30) Kabul"},
                     new SelectListItem{ Value="5",Text="(GMT + 5:00) Ekaterinburg, Islamabad, Karachi, Tashkent"},
                      new SelectListItem{ Value="5.5",Text="(GMT + 5:30) Mumbai, Kolkata, Chennai, New Delhi"},
                       new SelectListItem{ Value="5.75",Text="(GMT + 5:45) Kathmandu"},
                        new SelectListItem{ Value="6",Text="(GMT + 6:00) Almaty, Dhaka, Colombo"},
                         new SelectListItem{ Value="6.5",Text="(GMT + 6:30) Yangon, Cocos Islands"},
                          new SelectListItem{ Value="7",Text="(GMT + 7:00) Bangkok, Hanoi, Jakarta"},
                           new SelectListItem{ Value="8",Text="(GMT + 8:00) Beijing, Perth, Singapore, Hong Kong"},
                           new SelectListItem{ Value="9",Text="(GMT + 9:00) Tokyo, Seoul, Osaka, Sapporo, Yakutsk"},
                       new SelectListItem{ Value="9.5",Text="(GMT + 9:30) Adelaide, Darwin"},
                        new SelectListItem{ Value="10",Text="(GMT + 10:00) Eastern Australia, Guam, Vladivostok"},
                         new SelectListItem{ Value="11",Text="(GMT + 11:00) Magadan, Solomon Islands, New Caledonia"},
                          new SelectListItem{ Value="12",Text="(GMT + 12:00) Auckland, Wellington, Fiji, Kamchatka"},


             };
            myList = data.ToList();
            return myList;
        }

    }
}