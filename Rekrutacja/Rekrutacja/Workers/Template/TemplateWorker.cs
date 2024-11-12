using Soneta.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soneta.Kadry;
using Soneta.KadryPlace;
using Soneta.Types;
using Rekrutacja.Workers.Template;
using Rekrutacja.Constants;
using Soneta.Data.QueryDefinition;
using Soneta.Core;
using Rekrutacja.Extensions;

//Rejetracja Workera - Pierwszy TypeOf określa jakiego typu ma być wyświetlany Worker, Drugi parametr wskazuje na jakim Typie obiektów będzie wyświetlany Worker
[assembly: Worker(typeof(TemplateWorker), typeof(Pracownicy))]
namespace Rekrutacja.Workers.Template
{
    public class TemplateWorker
    {
        //Aby parametry działały prawidłowo dziedziczymy po klasie ContextBase
        public class TemplateWorkerParametry : ContextBase
        {

#if Zadanie1
            public int A { get; set; }

            public int B { get; set; }

            [Caption("Data obliczeń")]
            public Date DataObliczen { get; set; }

            public string Operacja { get; set; }
#endif

#if Zadanie2
            public int A { get; set; }

            public int B { get; set; }

            [Caption("Data obliczeń")]
            public Date DataObliczen { get; set; }

            public Figura Figura { get; set; }
#endif

#if Zadanie3
            public string A { get; set; }

            public string B { get; set; }

            [Caption("Data obliczeń")]
            public Date DataObliczen { get; set; }

            public Figura Figura { get; set; }
#endif

            public TemplateWorkerParametry(Context context) : base(context)
            {
                this.DataObliczen = Date.Today;
            }
        }
        //Obiekt Context jest to pudełko które przechowuje Typy danych, aktualnie załadowane w aplikacji
        //Atrybut Context pobiera z "Contextu" obiekty które aktualnie widzimy na ekranie
        [Context]
        public Context Cx { get; set; }
        //Pobieramy z Contextu parametry, jeżeli nie ma w Context Parametrów mechanizm sam utworzy nowy obiekt oraz wyświetli jego formatkę
        [Context]
        public TemplateWorkerParametry Parametry { get; set; }
        //Atrybut Action - Wywołuje nam metodę która znajduje się poniżej
        [Action("Kalkulator",
           Description = "Prosty kalkulator ",
           Priority = 10,
           Mode = ActionMode.ReadOnlySession,
           Icon = ActionIcon.Accept,
           Target = ActionTarget.ToolbarWithText)]
        public void WykonajAkcje()
        {
            //Włączenie Debug, aby działał należy wygenerować DLL w trybie DEBUG
            DebuggerSession.MarkLineAsBreakPoint();
            //Pobieranie danych z Contextu

            Pracownik[] pracownicy = null;
            if (this.Cx.Contains(typeof(Pracownik[])))
            {
                pracownicy = ((Pracownik[])this.Cx[typeof(Pracownik[])]);
            }

            //Modyfikacja danych
            //Aby modyfikować dane musimy mieć otwartą sesję, któa nie jest read only
            using (Session nowaSesja = this.Cx.Login.CreateSession(false, false, "ModyfikacjaPracownika"))
            {
                //Otwieramy Transaction aby można było edytować obiekt z sesji
                using (ITransaction trans = nowaSesja.Logout(true))
                {
                    foreach (var pracownik in pracownicy)
                    {
                        //Pobieramy obiekt z Nowo utworzonej sesji
                        var pracownikZSesja = nowaSesja.Get(pracownik);
                        //Features - są to pola rozszerzające obiekty w bazie danych, dzięki czemu nie jestesmy ogarniczeni to kolumn jakie zostały utworzone przez producenta
                        pracownikZSesja.Features["DataObliczen"] = this.Parametry.DataObliczen;

#if Zadanie1
                        switch (Parametry.Operacja)
                        {
                            case "+":
                                pracownikZSesja.Features["Wynik"] = (double)(this.Parametry.A + this.Parametry.B);
                                break;
                            case "-":
                                pracownikZSesja.Features["Wynik"] = (double)(this.Parametry.A - this.Parametry.B);
                                break;
                            case "*":
                                pracownikZSesja.Features["Wynik"] = (double)(this.Parametry.A * this.Parametry.B);
                                break;
                            case "/":
                                pracownikZSesja.Features["Wynik"] = (double)(this.Parametry.A) / (double)(this.Parametry.B == 0 ? 1 : this.Parametry.B);
                                break;
                            default:
                                break;
                        }
#endif

#if Zadanie2
                        switch (this.Parametry.Figura)
                        {
                            case Figura.Kwadrat:
                                if (Parametry.A != Parametry.B)
                                    throw new ArgumentException("Boki kwadratu muszą być równe.");
                                pracownikZSesja.Features["Wynik"] = (double)(this.Parametry.A * this.Parametry.B);
                                break;
                            case Figura.Prostokat:
                                pracownikZSesja.Features["Wynik"] = (double)(this.Parametry.A * this.Parametry.B);
                                break;
                            case Figura.Trojkat:
                                pracownikZSesja.Features["Wynik"] = (double)((this.Parametry.A * this.Parametry.B)/2);
                                break;
                            case Figura.Kolo:
                                pracownikZSesja.Features["Wynik"] = Math.Truncate((Math.PI * Math.Pow(this.Parametry.A, 2)));
                                break;
                            default:
                                break;
                        }
#endif

#if Zadanie3
                        switch (this.Parametry.Figura)
                        {
                            case Figura.Kwadrat:
                                if (Parametry.A != Parametry.B)
                                    throw new ArgumentException("Boki kwadratu muszą być równe.");
                                pracownikZSesja.Features["Wynik"] = (double)(this.Parametry.A.ToInt() * this.Parametry.B.ToInt());
                                break;
                            case Figura.Prostokąt:
                                pracownikZSesja.Features["Wynik"] = (double)(this.Parametry.A.ToInt() * this.Parametry.B.ToInt());
                                break;
                            case Figura.Trójkąt:
                                pracownikZSesja.Features["Wynik"] = (double)((this.Parametry.A.ToInt() * this.Parametry.B.ToInt()) / 2);
                                break;
                            case Figura.Koło:
                                pracownikZSesja.Features["Wynik"] = Math.Truncate((Math.PI * Math.Pow(this.Parametry.A.ToInt(), 2)));
                                break;
                            default:
                                break;
                        }
#endif
                    }

                    //Zatwierdzamy zmiany wykonane w sesji
                    trans.CommitUI();
                }
                //Zapisujemy zmiany
                nowaSesja.Save();
            }
        }
    }
}