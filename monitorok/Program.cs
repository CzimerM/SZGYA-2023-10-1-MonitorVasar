using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Channels;

namespace monitorvasar20230925
{
    class Keszlet
    {
        public Monitor monitor;
        public int mennyiseg;

        public Keszlet(Monitor monitor, int mennyiseg)
        {
            this.monitor = monitor;
            this.mennyiseg = mennyiseg;
        }

        public override string ToString()
        {
            return $"{this.monitor.Gyarto,10} {this.monitor.Tipus,25} | {this.mennyiseg, 3}";
        }
    }

    class Program
    {

        static int ezerbe(int a) => a / 1000;

        static Monitor ajanlo(List<Keszlet> keszlet)
        {
            int atlag = (int)keszlet.Average(m => m.monitor.Ar);
            var nagyobb = keszlet.FindAll(m => m.monitor.Ar > atlag);
            var mon = nagyobb.Where(m => m.monitor.Ar == nagyobb.Min(m => m.monitor.Ar)).First();
            return mon.monitor;
        }

        static void Main(string[] args)
        {

            List<Keszlet> keszlet = new List<Keszlet>();

            StreamReader sr = new StreamReader("../../../monitorok.txt", Encoding.UTF8);

            _ = sr.ReadLine();

            while (!sr.EndOfStream)
            {
                keszlet.Add(new Keszlet(new Monitor(sr.ReadLine()), 15));
            }


            //2. Írd ki a monitorok összes adatát virtuális metódussal, soronként egy monitort a képernyőre. A kiírás így nézzen ki:
             

            foreach (var item in keszlet)
            {
                Console.WriteLine(item.monitor);
            }
            Console.WriteLine();
            

            //2. Tárold az osztálypéldányokban a bruttó árat is (ÁFA: 27%, konkrétan a 27-tel számolj, ne 0,27-tel vagy más megoldással.)

            //3. Tételezzük fel, hogy mindegyik monitorból 15 db van készleten, ez a nyitókészlet. Mekkora a nyitó raktárkészlet bruttó (tehát áfával növelt) értéke?
            //Írj egy metódust, ami meghívásakor kiszámolja a raktárkészlet aktuális bruttó értékét. A főprogram írja ki az értéket.
            Console.WriteLine($"2.feladat: készlet bruttó ára: {keszlet.Sum(k => k.monitor.ArBrutto)} Ft\n");


            //4. Írd ki egy új fájlba, és a képernyőre az 50.000 Ft feletti nettó értékű monitorok összes adatát (a darabszámmal együtt) úgy
            //hogy a szöveges adatok nagybetűsek legyenek, illetve az árak ezer forintba legyenek átszámítva.
            //Az ezer forintba átszámítást egy külön függvényben valósítsd meg.
            Console.WriteLine("4.feladat");
            var sw = new StreamWriter("../../../dragamonitorok.txt", false, Encoding.UTF8);
            foreach (var monitor in keszlet)
            {
                if (monitor.monitor.Ar > 50000)
                {
                    Console.WriteLine($"{monitor.monitor.Gyarto.ToUpper()} {monitor.monitor.Tipus.ToUpper()} {ezerbe(monitor.monitor.Ar)}ezer Ft");
                }
            }
            Console.WriteLine();

            //5. Egy vevő keresi a HP EliteDisplay E242 monitort. Írd ki neki a képernyőre, hogy hány darab ilyen van a készleten.

            string keresett = "HP EliteDisplay E242";
            Console.WriteLine("5. feladat");
            var f5mon = keszlet.Find(m => m.monitor.Gyarto == keresett.Split(' ').First() && keresett.EndsWith(m.monitor.Tipus));
            if (f5mon != null) Console.WriteLine($"A keresett monitorból {f5mon.mennyiseg}db van készleten");
            //Ha nincs a készleten, ajánlj neki egy olyan monitort, aminek az ára az átlaghoz fölülről közelít. Ehhez használd az átlagszámító függvényt (később lesz feladat).
            else
            {
                var mon = ajanlo(keszlet);
                Console.WriteLine($"Sajnos nincs készleten. Helyette ajánluk a {mon.Gyarto} {mon.Tipus} monitort");
            }

            Console.WriteLine();
            //6. Egy újabb vevőt csak az ár érdekli. Írd ki neki a legolcsóbb monitor méretét, és árát.

            var f6monOlcso = keszlet.Find(m => m.monitor.Ar == keszlet.Min(m => m.monitor.Ar));
            Console.WriteLine($"6. feladat: A legolcsóbb monitor mérete {f6monOlcso.monitor.Meret} col, ára {f6monOlcso.monitor.Ar}");
            Console.WriteLine();
            //7. A cég akciót hirdet. A 70.000 Ft fölötti árú Samsung monitorok bruttó árából 5%-ot elenged.
            //Írd ki, hogy mennyit veszítene a cég az akcióval, ha az összes akciós monitort akciósan eladná.

            double f7Ossz = keszlet.Where(m => m.monitor.Ar > 70000 && m.monitor.Gyarto == "Samsung")
                .Sum(k => k.mennyiseg * k.monitor.Ar);

            double f7akciosOssz = keszlet.Where(m => m.monitor.Ar > 70000 && m.monitor.Gyarto == "Samsung")
                .Sum(k => k.mennyiseg * k.monitor.Ar * 0.95);


            Console.WriteLine($"7. feladat: A cég {Math.Round(f7Ossz - f7akciosOssz)} forintot veszítene");
            Console.WriteLine();
            //8. Írd ki a képernyőre minden monitor esetén, hogy az adott monitor nettó ára a nettó átlag ár alatt van-e, vagy fölötte,
            //esetleg pontosan egyenlő az átlag árral. Ezt is a főprogram írja ki.
            Console.WriteLine("8. feladat");
            int atlag = (int)keszlet.Average(m => m.monitor.Ar);
            foreach (var mon in keszlet)
            {
                if (mon.monitor.Ar > atlag) Console.WriteLine($"Felett: {mon.monitor.Gyarto} {mon.monitor.Tipus}");
                else if (mon.monitor.Ar < atlag) Console.WriteLine($"Alatt: {mon.monitor.Gyarto} {mon.monitor.Tipus}");
                else Console.WriteLine($"Annyi mint az átlag: {mon.monitor.Gyarto} {mon.monitor.Tipus}");
            }

            Console.WriteLine();
            //9. Modellezzük, hogy megrohamozták a vevők a boltot. 5 és 15 közötti random számú vásárló 1 vagy 2 random módon kiválasztott monitort vásárol
            //ezzel csökkentve az eredeti készletet. Írd ki, hogy melyik monitorból mennyi maradt a boltban.
            //Vigyázz, hogy nulla darab alá ne mehessen a készlet. Ha az adott monitor éppen elfogyott, ajánlj neki egy másikat (lásd fent).
            Console.WriteLine("9. feladat");
            Random rnd = new Random();
            for (int i = 0; i < rnd.Next(5, 16); i++)
            {
                for (int j = 0; j < rnd.Next(1, 2); j++)
                {
                    int index = rnd.Next(keszlet.Count);
                    if (keszlet[index].mennyiseg > 0) keszlet[index].mennyiseg--;
                    else
                    {
                        var mon = ajanlo(keszlet);
                        Console.WriteLine($"Sajnos nincs készleten. Helyette ajánluk a {mon.Gyarto} {mon.Tipus} monitort");
                    }
                }
            }

            Console.WriteLine();
            //10. Írd ki a képernyőre, hogy a vásárlások után van-e olyan monitor, amelyikből mindegyik elfogyott (igen/nem).
            
            Console.WriteLine("10.feladat");
            bool van = keszlet.Exists(k => k.mennyiseg == 0);
            if (van) Console.WriteLine("igen");
            else Console.WriteLine("nem");
            Console.WriteLine();
            //11. Írd ki a gyártókat abc sorrendben a képernyőre. Oldd meg úgy is, hogy a metódus írja ki, és úgy is, hogy a főprogram.
            Console.WriteLine("11.feladat");
            var gyartok = keszlet.Select(k => k.monitor.Gyarto).Distinct().ToList();
            gyartok.Sort();
            gyartok.ForEach(k => Console.WriteLine(k));
            Console.WriteLine();
            //12. Csökkentsd a legdrágább monitor bruttó árát 10%-kal, írd ki ezt az értéket a képernyőre.
            Console.WriteLine("12.feladat");
            var legdragabb = keszlet.Find(k => k.monitor.Ar == keszlet.Max(k => k.monitor.Ar));
            Console.WriteLine($"legdragabb.monitor.ArBrutto * 0.90 Ft");

            Console.ReadKey();
        }
    }
}
