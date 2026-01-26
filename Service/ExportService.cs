using BhawanaPatra.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BhawanaPatra.Service
{
    public class ExportService
    {
        public byte[] GeneratePdf(List<EntryModel> entries, string username)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(QuestPDF.Helpers.Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial").FontColor("#000000"));

                    // Header
                    page.Header()
                        .BorderBottom(1)
                        .BorderColor("#000000")
                        .PaddingBottom(10)
                        .Column(column =>
                        {
                            column.Item().Text("Bhawana Patra")
                                .FontSize(18)
                                .Bold()
                                .FontColor("#000000");

                            column.Item().Text($"Journal Entries - {username}")
                                .FontSize(10)
                                .FontColor("#000000");

                            column.Item().Text($"Exported: {DateTime.Now:MMMM dd, yyyy}")
                                .FontSize(9)
                                .FontColor("#000000");
                        });

                    // Content
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            if (entries == null || !entries.Any())
                            {
                                column.Item().Text("No entries to export.")
                                    .FontSize(12)
                                    .FontColor("#000000");
                                return;
                            }

                            foreach (var entry in entries.OrderByDescending(e => e.EntryDateKey))
                            {
                                column.Item().Element(container => ComposeEntry(container, entry));
                                column.Item().PaddingVertical(8);
                            }
                        });

                 
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }

        private void ComposeEntry(QuestPDF.Infrastructure.IContainer container, EntryModel entry)
        {
            container.BorderBottom(1).BorderColor("#000000").PaddingBottom(10).Column(column =>
            {
                // Date
                column.Item().Text(FormatDate(entry.EntryDateKey))
                    .FontSize(12)
                    .Bold()
                    .FontColor("#000000");

                // Title
                if (!string.IsNullOrWhiteSpace(entry.Title))
                {
                    column.Item().PaddingTop(3).Text(entry.Title)
                        .FontSize(11)
                        .SemiBold()
                        .FontColor("#000000");
                }

               

                // Mood
                if (!string.IsNullOrEmpty(entry.PrimaryMood))
                {
                    column.Item().PaddingTop(5).Text($"Mood: {entry.PrimaryMood}")
                        .FontSize(9)
                        .FontColor("#000000");
                }

                // Tags
                if (!string.IsNullOrEmpty(entry.Tags))
                {
                    var tagsList = string.Join(", ", entry.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()));
                    column.Item().PaddingTop(2).Text($"Tags: {tagsList}")
                        .FontSize(9)
                        .FontColor("#000000");
                }

                // Content
                if (!string.IsNullOrWhiteSpace(entry.Content))
                {
                    var plainText = StripHtml(entry.Content);
                    column.Item().PaddingTop(8).Text(plainText)
                        .FontSize(10)
                        .LineHeight(1.5f)
                        .FontColor("#000000");
                }
            });
        }

        private string FormatDate(string dateKey)
        {
            if (DateTime.TryParse(dateKey, out var date))
                return date.ToString("MMMM dd, yyyy");
            return dateKey;
        }

        private string StripHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            var text = System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
            text = System.Net.WebUtility.HtmlDecode(text);
            return text.Trim();
        }
    }
}