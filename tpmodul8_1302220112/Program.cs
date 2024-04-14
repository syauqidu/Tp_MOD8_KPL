using System;
using System.IO;
using Newtonsoft.Json;

class CovidConfig
{
    public string SatuanSuhu { get; set; }
    public int BatasHariDemam { get; set; }
    public string PesanDitolak { get; set; }
    public string PesanDiterima { get; set; }

    public CovidConfig()
    {
        // Set nilai default
        SatuanSuhu = "celcius";
        BatasHariDemam = 14;
        PesanDitolak = "Anda tidak diperbolehkan masuk ke dalam gedung ini";
        PesanDiterima = "Anda dipersilahkan untuk masuk ke dalam gedung ini";
    }

    public void BacaKonfigurasi(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            CovidConfig config = JsonConvert.DeserializeObject<CovidConfig>(json);
            SatuanSuhu = config.SatuanSuhu;
            BatasHariDemam = config.BatasHariDemam;
            PesanDitolak = config.PesanDitolak;
            PesanDiterima = config.PesanDiterima;
        }
    }

    public void UbahSatuan()
    {
        SatuanSuhu = SatuanSuhu == "celcius" ? "fahrenheit" : "celcius";
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Membaca konfigurasi dari file
        CovidConfig config = new CovidConfig();
        config.BacaKonfigurasi("covid_config.json");

        // Meminta input dari pengguna
        Console.WriteLine($"Berapa suhu badan anda saat ini? Dalam nilai {config.SatuanSuhu}: ");
        double suhu = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam?");
        int hariDemam = Convert.ToInt32(Console.ReadLine());

        // Memeriksa kondisi
        bool kondisiSuhu = (config.SatuanSuhu == "celcius" && suhu >= 36.5 && suhu <= 37.5) ||
                           (config.SatuanSuhu == "fahrenheit" && suhu >= 97.7 && suhu <= 99.5);
        bool kondisiHari = hariDemam < config.BatasHariDemam;

        // Mengeluarkan output sesuai kondisi
        if (kondisiSuhu && kondisiHari)
        {
            Console.WriteLine(config.PesanDiterima);
        }
        else
        {
            Console.WriteLine(config.PesanDitolak);
        }

        // Panggil method untuk mengubah satuan suhu
        config.UbahSatuan();
        Console.WriteLine($"Satuan suhu telah diubah menjadi: {config.SatuanSuhu}");
    }
}
