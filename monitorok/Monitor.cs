namespace monitorvasar20230925;

class Monitor
{
    public string Gyarto { get; set; }
    public string Tipus { get; set; }
    public int Ar { get; set; }
    public float Meret { get; set; }

    public float ArBrutto => this.Ar + this.Ar / 100f * 27f;

    public override string ToString()
    {
        return $"Gyártó: {this.Gyarto}; Típus: {this.Tipus}; Méret: {this.Meret} col; Nettó ár: {this.Ar} Ft";
    }

    public Monitor(string sor)
    {
        string[] adatok = sor.Split(';');
        this.Gyarto = adatok[0];
        this.Tipus = adatok[1];
        this.Meret = float.Parse(adatok[2].Replace('.',','));
        this.Ar = int.Parse(adatok[3]);

    }
}