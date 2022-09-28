using Application.Services.Repositories;
using Core.CrossCuttingConcers.Exceptions;
using Core.Persistence.Paging;
using Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProgrammingLanguages.Rules
{
    public class ProgrammingLanguageBusinessRules
    {

        private readonly IProgrammingLanguageRepository _programmingLanguageRepository;

        public ProgrammingLanguageBusinessRules(IProgrammingLanguageRepository programmingLanguageRepository)
        {
            _programmingLanguageRepository = programmingLanguageRepository;
        }

        public async Task ProgrammingLanguageNameCanNotBeDuplicatedWhenInserted(string name)
        {
           IPaginate<ProgrammingLanguage> result = await _programmingLanguageRepository.GetListAsync(pl => pl.Name == name);
            if (result.Items.Any())
                throw new BusinessException("Programming Name exists!");
        }

        public void ProgrammingLanguageShouldExistWhenRequested(ProgrammingLanguage programmingLanguage)
        {
            if (programmingLanguage == null)
                throw new BusinessException("Requested programming language does not exit!");
        }

        public async Task ProgrammingLanguageIdShouldExist(int id)
        {
            IPaginate<ProgrammingLanguage> result = await _programmingLanguageRepository.GetListAsync(pl => pl.Id == id);
            if (result.Items.Count == 0 || id == 0)
                throw new BusinessException("The entered id is invalid, please enter valid id!");
        }
    }
}
