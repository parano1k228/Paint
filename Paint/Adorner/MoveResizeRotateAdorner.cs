﻿using Paint.AdornerTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paint.Adorner
{
    public class MoveResizeRotateAdorner : AdornerWithTemplate
    {
        public MoveResizeRotateAdorner(UIElement adornedElement) :
            base(
                adornedElement,
                new MoveResizeRotateAdornerTemplate()
            )
        {
        }
    }
}
