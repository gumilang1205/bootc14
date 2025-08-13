using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using EntityFramework.Data;
using (var db = new FacultyContext())
{
    //menambah fakultas
    // var fakultas = new Fakultas { NamaFakultas = "Fakultas Ekonomi dan Bisnis" };
    // var prodi = new Prodi { NamaProdi = "Akutansi", Fakultas = fakultas };
    // db.Fakultas.Add(fakultas);
    // db.SaveChanges();

    // var mahasiswa = new Mahasiswa
    // {
    //     NamaMahasiswa = "Pairno",
    //     NIM = "133222455",
    //     TanggalLahir = new DateTime(2010, 1, 1),
    //     Prodi = prodi
    // };
    // db.Mahasiswa.Add(mahasiswa);
    // db.SaveChanges();
    // Console.WriteLine("Berhasil menambahkan data");


    //menambah fakultas
    // var fakultasFEB = new Fakultas { NamaFakultas = "Fakultas Ekonomi dan Bisnis" };
    // var prodiAkutansi = new Prodi { NamaProdi = "Akutansi", Fakultas = fakultas };
    // db.Fakultas.Remove(fakultas);
    // db.SaveChanges();

    // var mahasiswaPairno = new Mahasiswa
    // {
    //     NamaMahasiswa = "Pairno",
    //     NIM = "133222455",
    //     TanggalLahir = new DateTime(2010, 1, 1),
    //     Prodi = prodi
    // };
    // db.Mahasiswa.Remove(mahasiswa);
    // db.SaveChanges();
    // Console.WriteLine("Berhasil menghapus data");
    // var fakultasRemove = db.Fakultas.FirstOrDefault(e => e.FakultasID == 17);
    // db.Fakultas.Remove(fakultasRemove);
    // db.SaveChanges();
    // Console.WriteLine("Berhasil menghapus data fakultas");

    // var fakultasRead = db.Fakultas.FirstOrDefault(e => e.FakultasID == 16);
    // Console.WriteLine(fakultasRead.NamaFakultas);
    // var fakultasUpdate = db.Fakultas.FirstOrDefault(e => e.FakultasID == 5);
    // fakultasUpdate.NamaFakultas = "Fakultas Kedokteran dan Keperawatan";
    // db.SaveChanges();
    // Console.WriteLine("Berhasil mengupdate data fakultas");

}