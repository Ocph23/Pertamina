using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Ocph.DAL;
using Ocph.DAL.Repository;
using WebApp.Models;
using MySql.Data.MySqlClient;

namespace WebApp
{
    public class OcphDbContext : Ocph.DAL.Provider.MySql.MySqlDbConnection
    {

        public OcphDbContext(string connection)
        {
            ConnectionString = connection;
        }

        // public IRepository<Karyawan> Karyawan { get { return new Repository<Karyawan>(this); } }
        // public IRepository<Pointkaryawan> PoinKaryawan { get { return new Repository<Pointkaryawan>(this); } }
        // public IRepository<Perusahaan> Perusahaan { get { return new Repository<Perusahaan>(this); } }
        // public IRepository<Pointperusahaan> PointPerusahaan { get { return new Repository<Pointperusahaan>(this); } }
        // public IRepository<Absen> Absens { get { return new Repository<Absen>(this); } }
        // public IRepository<Jenispelanggaran> JenisPelanggaran { get { return new Repository<Jenispelanggaran>(this); } }
        // public IRepository<Pelanggaran> Pelanggaran { get { return new Repository<Pelanggaran>(this); } }
        // public IRepository<DataFile> BuktiPelanggaran { get { return new Repository<DataFile>(this); } }
        // public IRepository<Pemenang> Pemenang { get { return new Repository<Pemenang>(this); } }
        // public IRepository<Pengaduan> Pengaduan { get { return new Repository<Pengaduan>(this); } }
        // public IRepository<Periode> Periode { get { return new Repository<Periode>(this); } }
        // public IRepository<Terlapor> Terlapor { get { return new Repository<Terlapor>(this); } }
        // public IRepository<Level> Level { get { return new Repository<Level>(this); } }
        public IEnumerable<dynamic> SelectDynamic(string sql)
        {

            var command = this.CreateCommand();
            command.CommandText = sql;
            command.CommandType = System.Data.CommandType.Text;
            using (var reader = command.ExecuteReader())
            {
                var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                foreach (IDataRecord record in reader as IEnumerable)
                {
                    var expando = new Dictionary<string, object>();
                    foreach (var name in names)
                        expando[name] = record[name];

                    yield return expando;
                }
            }
        }

        public int InsertGetId(string sql)
        {
            try
            {
                var command = this.CreateCommand();
                command.CommandText = sql;
                command.CommandType = System.Data.CommandType.Text;
                var result = Convert.ToInt32(command.ExecuteScalar());
                if (result > 0)
                    return result;
                else throw new SystemException("Data Tidak Berhasil Disimpan");
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }

        }


        public bool Delete(string sql)
        {

            try
            {
                var command = this.CreateCommand();
                command.CommandText = sql;
                command.CommandType = System.Data.CommandType.Text;
                var result = Convert.ToInt32(command.ExecuteScalar());
                if (result > 0)
                    return true;
                else throw new SystemException("Data Tidak Berhasil Dihapus");
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }

        }

        public bool Update(string sql)
        {
            try
            {
                var command = this.CreateCommand();
                command.CommandText = sql;
                command.CommandType = System.Data.CommandType.Text;
                var result = Convert.ToInt32(command.ExecuteScalar());
                if (result > 0)
                    return true;
                else throw new SystemException("Data Tidak Berhasil Diubah");
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }

        }


        public IEnumerable<T> Query<T>(string sql)
        {
            var command = this.CreateCommand();
            command.CommandText = sql;
            command.CommandType = System.Data.CommandType.Text;
            var reader = command.ExecuteReader();
            var results = Ocph.DAL.Mapping.MySql.MappingProperties<T>.MappingTable(reader);
            reader.Close();
            return results.ToList();
        }
    }
}