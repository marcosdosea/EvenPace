using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Service
{
    public class InscricaoService : IInscricaoService
    {
        private EvenPaceContext _context;


        public void Cancelar(int idInscricao, int idCorredor)
        {
            var inscricao = _context.Inscricaos
                .Include(i => i.IdKit)
                .FirstOrDefault(i => i.Id == idInscricao && i.IdCorredor == idCorredor);

            if (inscricao == null)
                throw new Exception("Inscrição não encontrada ou não pertence ao corredor.");

            
            if (inscricao.DataInscricao < DateTime.Now)
                throw new Exception("Não é possível cancelar após a data do evento.");

            if (inscricao.Status == "Cancelada")
                return;

            inscricao.Status = "Cancelada";

            _context.Update(inscricao);
            _context.SaveChanges();
        }

        public InscricaoService(EvenPaceContext context)
        {
            _context = context;
        }

        public int Create(Inscricao inscricao)
        {
            _context.Add(inscricao);
            _context.SaveChanges();
            return inscricao.Id;
        }

        public void Edit(Inscricao inscricao)
        {
            _context.Update(inscricao);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var _inscricao = _context.Inscricaos.Find(id);
            _context.Remove(_inscricao);
            _context.SaveChanges();            
        }

        public Inscricao Get(int id)
        {
            return _context.Inscricaos.FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<Inscricao> GetAll()
        {
            return _context.Inscricaos;
        }

        public IEnumerable<Inscricao> GetAllByEvento(int idEvento)
        {
            return _context.Inscricaos
                .Include(i => i.IdKitNavigation)
                .Include(i => i.IdCorredorNavigation)
                .Include(i => i.IdEventoNavigation)
                .Where(i => i.IdEvento == idEvento)
                .ToList();
        }
    }
}
