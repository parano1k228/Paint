﻿using Paint.Gestures;
using Paint.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Paint.CustomControl
{
    public class DesignItemsControl : ItemsControl
    {
        private Selecting _selectingGesture { get; set; }
        private Gestures.Drawing _drawingGesture;

        public DesignCanvas DesignCanvas { get; set; }

        public bool IsDrawing { get; set; }

        static DesignItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignItemsControl), new FrameworkPropertyMetadata(typeof(DesignItemsControl)));
        }

        public DesignItemsControl()
        {
            DraggingAttached.Context = this;
            _selectingGesture = new Selecting(this);
            _drawingGesture = new Gestures.Drawing(this);

            DraggingAttached.DragDelta += HandleDragDelta;
        }

        // Curent Node for drawing
        public static readonly DependencyProperty DrawingNodeProperty =
            DependencyProperty.Register(
                "DrawingNode",
                typeof(NodeViewModel),
                typeof(DesignItemsControl),
                new UIPropertyMetadata(null)
       );

        public NodeViewModel DrawingNode
        {
            get { return (NodeViewModel)GetValue(DrawingNodeProperty); }
            set { SetValue(DrawingNodeProperty, value); }
        }

        // Selection Rectngle
        public static readonly DependencyProperty SelectionRectangleProperty =
             DependencyProperty.Register(
                 "SelectionRectangle",
                 typeof(Rect),
                 typeof(DesignItemsControl),
                 new UIPropertyMetadata(null)
        );

        public Rect SelectionRectangle
        {
            get { return (Rect)GetValue(SelectionRectangleProperty); }
            set { SetValue(SelectionRectangleProperty, value); }
        }

        // Is any item is dragging
        public static readonly DependencyProperty IsDraggingProperty =
            DependencyProperty.Register(
                "IsDragging",
                typeof(bool),
                typeof(DesignItemsControl),
                new UIPropertyMetadata(false)
       );

        public bool IsDragging
        {
            get { return (bool)GetValue(IsDraggingProperty); }
            set { SetValue(IsDraggingProperty, value); }
        }

        // Selecting
        public static readonly DependencyProperty IsAreaSelectingProperty =
             DependencyProperty.Register(
                 "IsAreaSelecting",
                 typeof(bool),
                 typeof(DesignItemsControl),
                 new UIPropertyMetadata(false)
        );

        public bool IsAreaSelecting
        {
            get { return (bool)GetValue(IsAreaSelectingProperty); }
            set { SetValue(IsAreaSelectingProperty, value); }
        }

        // Selected Items
        public static readonly DependencyProperty SelectedItemsProperty =
             DependencyProperty.Register(
                 "SelectedItems",
                 typeof(ObservableCollection<NodeViewModel>),
                 typeof(DesignItemsControl),
                 new UIPropertyMetadata(new ObservableCollection<NodeViewModel>())
        );

        public ObservableCollection<NodeViewModel> SelectedItems
        {
            get { return (ObservableCollection<NodeViewModel>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DesignItemContainer;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DesignItemContainer
            {
                RenderTransform = new TransformGroup
                {
                    Children = new TransformCollection(
                        new Transform[]
                        {
                            new ScaleTransform(),
                            new TranslateTransform()
                        })
                }
            };
        }


        private void HandleDragDelta(DesignItemContainer sender, Point point)
        {
            IsDragging = true;

            foreach (var item in SelectedItems)
            {
                var rotateTransform = new RotateTransform
                (
                    item.RotateAngle,
                    item.TransformOrigin.X,
                    item.TransformOrigin.Y
                );

                Point dragDelta = rotateTransform.Transform(point);

                item.Left += dragDelta.X;
                item.Top += dragDelta.Y;
            }
        }


        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            _selectingGesture.SelectByMouseDown(e);

            //if (DrawingNode != null)
            //{
            //    IsDrawing = true;
            //    DrawingNode.IsDrawing = true;
            //    _drawingGesture.OnMouseDown(e.GetPosition(this));
            //    return;
            //}

            //if (IsAreaSelecting)
            //{
            //    _selectingGesture.OnMouseDownStartAreaSelect(e.GetPosition(this));
            //}

            //if (IsDragging)
            //{
            //    Debug.WriteLine("drgaggging");
            //    return;
            //}
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            //if (IsAreaSelecting && !IsDragging)
            //{
            //    _selectingGesture.OnMouseMoveAreaSelecting(e.GetPosition(this));
            //}

            //if (IsDrawing)
            //{
            //    _drawingGesture.OnMouseMove(e.GetPosition(this));
            //}

            //if (IsDrawing)
            //{
            //    _drawingGesture.OnMouseMove(e.GetPosition(this));
            //}

            //if (IsAreaSelecting)
            //{
            //    _selectingGesture.OnMouseMoveAreaSelecting(e.GetPosition(this));
            //}
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            Debug.WriteLine("isDragging");
            Debug.WriteLine(IsDragging);
            
            if (!IsDragging)
            {
                _selectingGesture.SelectByMouseUp(e);
            }

            IsDragging = false;

            //if (IsDragging)
            //{
            //    IsDragging = false;
            //}



            //if (IsAreaSelecting)
            //{
            //    _selectingGesture.OnMouseUpEndSelecting(e.GetPosition(this));
            //    IsAreaSelecting = false;
            //    return;
            //}

            //if (IsDrawing)
            //{
            //    _drawingGesture.OnMouseUp(e.GetPosition(this));
            //    IsDrawing = false;
            //    return;
            //}

            //if (!IsAreaSelecting && !IsDrawing)
            //{
            //    Debug.WriteLine("fjdkfdfjdkfkjselecting");
            //    _selectingGesture.SelectByMouseDown(e);
            //}

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            DesignCanvas = (DesignCanvas)GetTemplateChild("PART_Canvas");
        }

        private void EventAnalyzeOnButtonDown(MouseButtonEventArgs e)
        {
            //if (DrawingNode != null)
            //{
            //    IsDrawing = true;
            //    DrawingNode.IsDrawing = true;
            //}

        }
    }
}