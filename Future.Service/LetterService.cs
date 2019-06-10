using Future.Model.Utils;
using Future.Repository;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Future.Service
{
    public class LetterService
    {
        private readonly LetterRepository letterDal = SingletonProvider<LetterRepository>.Instance;

        public ResponseContext<PickUpListResponse> PickUpList(RequestContext<PickUpListRequest> request)
        {
            var response = new ResponseContext<PickUpListResponse>()
            {
                Content = new PickUpListResponse()
            };
            throw new NotImplementedException();
        }
    }
}
