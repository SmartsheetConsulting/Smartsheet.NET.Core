﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Smartsheet.NET.Core.Entities
{
	public class Row : SmartsheetObject
	{
		public Row()
		{
			this.Cells = new List<Cell>();
			this.Columns = new List<Column>();
		}

		public Row Build(bool? preserveId = false, bool? strict = false, bool? toTop = null, bool? toBottom = null, bool? above = null, long? parentId = null, long? siblingId = null, IList<Cell> cells = null)
		{
			this.ToTop = toTop;
			this.ToBottom = toBottom;
			this.Above = above;
			this.ParentId = parentId;
			this.SiblingId = siblingId;

			this.RowNumber = null;
			this.CreatedAt = null;
			this.ModifiedAt = null;
			this.LockedForUser = null;
			this.Columns = null;
			this.Discussions = null;
			this.Attatchments = null;

			if (!preserveId.GetValueOrDefault())
			{
				this.Id = null;
			}

			if (cells != null && cells.Any())
			{
				this.Cells = cells;
			}

			var buildCells = new List<Cell>();

			for (var i = 0; i < this.Cells.Count; i++)
			{
				if (this.Cells[i].Value != null || this.Cells[i].Formula != null)
				{
					buildCells.Add(this.Cells[i].Build(strict));
				}
			}

			this.Cells = buildCells;

			return this;
		}

		public long? Id { get; set; }
		public long? SheetId { get; set; }
		public long? ParentId { get; set; }
		public long? SiblingId { get; set; }

		public int? RowNumber { get; set; }
		public int? Version { get; set; }

		public bool? FilteredOut { get; set; }
		public bool? InCriticalPath { get; set; }
		public bool? Locked { get; set; }
		public bool? LockedForUser { get; set; }
		public bool? Expanded { get; set; }
		public bool? ToTop { get; set; }
		public bool? ToBottom { get; set; }
		public bool? Above { get; set; }

		public string AccessLevel { get; set; }
		public string Format { get; set; }
		public string ConditionalFormat { get; set; }
		public string Permalink { get; set; }

		public DateTime? CreatedAt { get; set; }
		public User CreatedBy { get; set; }
		public DateTime? ModifiedAt { get; set; }
		public User ModifiedBy { get; set; }

		public IList<Cell> Cells { get; set; }
		public IList<Column> Columns { get; set; }

		public IList<Discussion> Discussions { get; set; }
		public IList<Attachment> Attatchments { get; set; }

		//
		//  Extension Methods
		#region Extensions
		public Cell GetCellForColumn(long columnId)
		{
			var cell = this.Cells.Where(c => c.Column.Id == columnId).FirstOrDefault();

			return cell;
		}

		public Cell GetCellForColumn(string columnTitle)
		{
			var cell = this.Cells.Where(c => c.Column.Title.Trim().ToLower() == columnTitle.ToLower()).FirstOrDefault();

			return cell;
		}

		public string GetValueForColumnAsString(string columnTitle)
		{
			var cell = this.Cells.SingleOrDefault(c => c.Column.Title.Trim().ToLower() == columnTitle.ToLower());
			return Convert.ToString(cell?.Value ?? "");
		}

		public void UpdateCellForColumn(string columnTitle, dynamic value)
		{
			var cell = this.Cells.Where(c => c.Column.Title.Trim().ToLower() == columnTitle.ToLower()).FirstOrDefault();

			cell.Value = value;
		}
		#endregion
	}
}
