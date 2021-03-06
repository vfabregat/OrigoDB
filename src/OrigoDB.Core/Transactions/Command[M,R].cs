﻿using System;

namespace OrigoDB.Core
{

    [Serializable]
    public abstract class Command<TModel, TResult> : Command, IOperationWithResult where TModel : Model
    {

        protected Command(bool ensuresResultIsDisconnected = false)
        {
            ResultIsSafe = ensuresResultIsDisconnected;
        }
        
        /// <summary>
        /// True if results are safe to return to client, default is false. Set to true if your command implementation 
        /// gaurantees no references to mutable objects within the model are returned.
        /// </summary>
        public bool ResultIsSafe{ get; internal protected set; }


        public virtual void Prepare(TModel model) { }


        //TODO: Duplicate method! refactor
        internal override void PrepareStub(Model model)
        {
            try
            {
                Prepare(model as TModel);
            }
            catch (Exception ex)
            {

                throw new CommandAbortedException("Exception thrown during Prepare(), see inner exception for details", ex);
            }
        }

        internal override object ExecuteStub(Model model)
        {
            return Execute(model as TModel);
        }

        public abstract TResult Execute(TModel model);
    }
}
