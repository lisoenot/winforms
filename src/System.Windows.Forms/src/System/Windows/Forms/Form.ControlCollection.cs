﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Windows.Forms
{
    public partial class Form
    {
        /// <summary>
        ///  Represents a collection of controls on the form.
        /// </summary>
        public new class ControlCollection : Control.ControlCollection
        {
            private readonly Form _owner;

            /*C#r:protected*/

            /// <summary>
            ///  Initializes a new instance of the ControlCollection class.
            /// </summary>
            public ControlCollection(Form owner)
                : base(owner)
            {
                _owner = owner;
            }

            /// <summary>
            ///  Adds a control
            ///  to the form.
            /// </summary>
            public override void Add(Control? value)
            {
                if (value is MdiClient && _owner.ctlClient is null)
                {
                    if (!_owner.TopLevel && !_owner.DesignMode)
                    {
                        throw new ArgumentException(SR.MDIContainerMustBeTopLevel, nameof(value));
                    }

                    _owner.AutoScroll = false;
                    if (_owner.IsMdiChild)
                    {
                        throw new ArgumentException(SR.FormMDIParentAndChild, nameof(value));
                    }

                    _owner.ctlClient = (MdiClient)value;
                }

                // make sure we don't add a form that has a valid mdi parent
                if (value is Form && ((Form)value).MdiParentInternal is not null)
                {
                    throw new ArgumentException(SR.FormMDIParentCannotAdd, nameof(value));
                }

                base.Add(value);

                if (_owner.ctlClient is not null)
                {
                    _owner.ctlClient.SendToBack();
                }
            }

            /// <summary>
            ///  Removes a control from the form.
            /// </summary>
            public override void Remove(Control? value)
            {
                if (value == _owner.ctlClient)
                {
                    _owner.ctlClient = null;
                }

                base.Remove(value);
            }
        }
    }
}
