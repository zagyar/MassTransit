// Copyright 2007-2015 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Saga.Policies
{
    using System;


    public class InitiatingSagaPolicy<TSaga, TMessage> :
        SagaPolicyBase<TSaga>,
        ISagaPolicy<TSaga, TMessage>
        where TSaga : class, ISaga
        where TMessage : class
    {
        readonly Func<ConsumeContext<TMessage>, Guid> _getNewSagaId;

        public InitiatingSagaPolicy(Func<ConsumeContext<TMessage>, Guid> getNewSagaId, Func<TSaga, bool> canRemoveInstance = null)
            : base(canRemoveInstance)
        {
            _getNewSagaId = getNewSagaId;
        }

        public bool CanCreateInstance(ConsumeContext<TMessage> context)
        {
            return true;
        }

        public TSaga CreateInstance(ConsumeContext<TMessage> context, Guid sagaId)
        {
            return SagaMetadataCache<TSaga>.FactoryMethod(sagaId);
        }

        public Guid GetNewSagaId(ConsumeContext<TMessage> context)
        {
            Guid sagaId = _getNewSagaId(context);

            return sagaId;
        }

        public bool CanUseExistingInstance(ConsumeContext<TMessage> context)
        {
            throw new SagaException("The message cannot be accepted by an existing saga", typeof(TSaga), typeof(TMessage));
        }
    }
}