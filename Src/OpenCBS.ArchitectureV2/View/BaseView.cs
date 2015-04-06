﻿using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using OpenCBS.ArchitectureV2.Interface.Service;
using OpenCBS.Controls;

namespace OpenCBS.ArchitectureV2.View
{
    public partial class BaseView : Form
    {
        private readonly ITranslationService _translationService;

        public BaseView() : this(null)
        {
        }

        public BaseView(ITranslationService translationService)
        {
            _translationService = translationService;
            Font = SystemFonts.MessageBoxFont;
            InitializeComponent();
        }

        public void Translate()
        {
            var validTypes = new[]
            {
                typeof (Label),
                typeof (Button),
                typeof (SplitButton),
                typeof (CheckBox),
                typeof (RadioButton),
                typeof (GroupBox),
                typeof (TabControl),
                typeof (ToolStripMenuItem),
                typeof (LinkLabel),
                typeof (OLVColumn)
            };

            var fields = GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(fi => fi.GetValue(this))
                .Where(v => v != null && validTypes.Contains(v.GetType()))
                .ToList();

            foreach (var field in fields)
            {
                var text = (string) field.GetType().GetProperty("Text").GetValue(field, null);
                text = _translationService.Translate(text);
                field.GetType().GetProperty("Text").SetValue(field, text, null);
            }
            Text = _translationService.Translate(Text);

            Invalidate(true);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            Translate();
        }
    }
}
