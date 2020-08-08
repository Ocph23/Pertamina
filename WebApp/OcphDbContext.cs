using Microsoft.Extensions.Configuration;
using Ocph.DAL.Repository;
using WebApp.Models;

namespace WebApp {
    public class OcphDbContext : Ocph.DAL.Provider.MySql.MySqlDbConnection {

        public OcphDbContext (string connection) {
            ConnectionString = connection;
        }

        public IRepository<Karyawan> Karyawan { get { return new Repository<Karyawan> (this); } }
        public IRepository<Pointkaryawan> PoinKaryawan { get { return new Repository<Pointkaryawan> (this); } }
        public IRepository<Perusahaan> Perusahaan { get { return new Repository<Perusahaan> (this); } }
        public IRepository<Pointperusahaan> PointPerusahaan { get { return new Repository<Pointperusahaan> (this); } }
        public IRepository<Absen> Absens { get { return new Repository<Absen> (this); } }
        public IRepository<Jenispelanggaran> JenisPelanggaran { get { return new Repository<Jenispelanggaran> (this); } }
        public IRepository<Pelanggaran> Pelanggaran { get { return new Repository<Pelanggaran> (this); } }
        public IRepository<Pemenang> Pemenang { get { return new Repository<Pemenang> (this); } }
        public IRepository<Pengaduan> Pengaduan { get { return new Repository<Pengaduan> (this); } }
        public IRepository<Periode> Periode { get { return new Repository<Periode> (this); } }
        public IRepository<Terlapor> Terlapor { get { return new Repository<Terlapor> (this); } }
        public IRepository<Level> Level { get { return new Repository<Level> (this); } }

    }
}