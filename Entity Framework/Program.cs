using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using EntityFramework.Data;
using (var db = new FacultyContext())
{
    //menambah fakultas
    var fakultas = new Fakultas { NamaFakultas = "Fakultas Ekonomi dan Bisnis" };
    var prodi = new Prodi { NamaProdi = "Akutansi", Fakultas = fakultas };
    db.Fakultas.Add(fakultas);
    db.SaveChanges();

    var mahasiswa = new Mahasiswa
    {
        NamaMahasiswa = "Pairno",
        NIM = "133222455",
        TanggalLahir = new DateTime(2010, 1, 1),
        Prodi = prodi
    };
    

}