using Example.EventHandler;
using Example.InterfaceX;

var bayar = new KartuDebit();
bayar.Bayar();
bayar.Nama("Babi");

var lampu = new Lampu();
var tombol = new Tombol();
tombol.Press += lampu.LampuMobil;
tombol.Press += lampu.LampuMotor;
tombol.Tekan();