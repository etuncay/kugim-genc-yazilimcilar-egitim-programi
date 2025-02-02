﻿using AutoMapper;
using Haber.Core.Interfaces.Services;
using Haber.Core.Validator;
using Haber.Data;
using Haber.Models.Enums;
using Haber.Models.ViewModels;
using Haber.Models.ViewModels.Request;
using Haber.Models.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haber.Services
{
    public class KategoriService : IKategoriService
    {
        private readonly HaberDbContext _haberDbContext;
        private readonly IMapper _mapper;

        public KategoriService(
            HaberDbContext haberDbContext,
            IMapper mapper)
        {
            _haberDbContext = haberDbContext;
            _mapper = mapper;
        }


        public ResponseResultModel<int> Ekle(KategoriRequestViewModel model)
        {
            var validator = new KategoriRequestValidator();

            var validate = validator.Validate(model);

            var result = new ResponseResultModel<int>();

            if (validate.IsValid)
            {
                var slug = new Slugify.SlugHelper();

                var entity = _mapper.Map<KategoriEntity>(model);
                entity.Slug = slug.GenerateSlug(entity.Ad);
                _haberDbContext.Add(entity);
                result.SaveChange(_haberDbContext.SaveChanges());
                result.Data = entity.Id;
                
            }
            else
            {
                result.SetErrors(validate);
            }

            return result;
        }

        public ResponseResultModel<List<KategoriResponseViewModel>> Listele()
        {
            var result = new ResponseResultModel<List<KategoriResponseViewModel>>();

            var queries = _haberDbContext.Kategori.ToList();

            if (queries.Any())
            {
                result.Data = _mapper.Map<List<KategoriResponseViewModel>>(queries);
                result.Message = ResponseResultMessageType.KayitBulundu;
                result.Type = EnumResponseResultType.Success;
                result.TotalCount = result.Data.Count();

            }
            else
            {
                result.Message = ResponseResultMessageType.KayitBulunamadi;
                result.Type = EnumResponseResultType.Error;
            }


            return result;
        }

        public ResponseResultModel<KategoriResponseViewModel> Getir(int id)
        {
            var result = new ResponseResultModel<KategoriResponseViewModel>();
            var query = _haberDbContext.Kategori.FirstOrDefault(q => q.Id == id);
            if (query != null)
            {
                result.Data = _mapper.Map<KategoriResponseViewModel>(query);

                result.Message = ResponseResultMessageType.KayitBulundu;
                result.Type = EnumResponseResultType.Success;

            }
            else
            {
                result.Message = ResponseResultMessageType.KayitBulunamadi;
                result.Type = EnumResponseResultType.Error;
            }

            return result;
        }

        public ResponseResultModel Guncelle(int id, KategoriRequestViewModel model)
        {
            var result = new ResponseResultModel();

            var query = _haberDbContext.Kategori.FirstOrDefault(q => q.Id == id);
            if (query != null)
            {
                var slug = new Slugify.SlugHelper();
                query.Ad = model.Ad;
                query.Slug = slug.GenerateSlug(query.Ad);
                query.Aciklama = model.Aciklama;
                query.GuncellenmeTarihi = DateTime.Now;
                result.SaveChange(_haberDbContext.SaveChanges());

            }
            else
            {
                result.Message = ResponseResultMessageType.KayitBulunamadi;
                result.Type = EnumResponseResultType.Error;
            }

            return result;
        }

        

        public ResponseResultModel Sil(int id)
        {
            var result = new ResponseResultModel();

            var query = _haberDbContext.Kategori.FirstOrDefault(q => q.Id == id);
            if (query != null)
            {
                _haberDbContext.Kategori.Remove(query);
                result.SaveChange(_haberDbContext.SaveChanges());
            }
            else
            {
                result.Message = ResponseResultMessageType.KayitBulunamadi;
                result.Type = EnumResponseResultType.Error;
            }

            return result;
        }
    }
}
