using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Service;

namespace Service
{
    public class AdministradorService : IAdministradorService
    {
        private readonly EvenPaceContext _context;

        public AdministradorService(EvenPaceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Insere um administrador no banco de dados
        /// </summary>
        /// <param name="administrador"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public uint Create(Administrador administrador)
        {
            _context.Add(administrador);
            _context.SaveChanges();
            return(uint) administrador.Id;
        }

        /// <summary>
        /// Deleta um administrador do banco de dados
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Delete(int id)
        {
            var _administrador = _context.Administradors.Find(id);

            if (_administrador is not null)
            {
                _context.Remove(_administrador);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Edita um administrador no banco de dados
        /// </summary>
        /// <param name="administrador"></param>
        public void Edit(Administrador administrador)
        {
            if (administrador is not null)
            {
                _context.Administradors.Find(administrador.Id);
                _context.Update(administrador);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Busca um administrador pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Administrador Get(int id)
        {
            return _context.Administradors.Find(id);
        }

        /// <summary>
        /// Pega todos os administradores do banco de dados
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Administrador> GetAll()
        {
            return _context.Administradors.ToList();
        }

        /// <summary>
        /// Pega administradores pelo nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public IEnumerable<Administrador> GetByName(string nome)
        {
            return _context.Administradors.Where(a => a.Nome.Contains(nome)).ToList();
        }
        
    }
}
