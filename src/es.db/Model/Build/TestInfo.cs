﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace es.Model {
	[JsonObject(MemberSerialization.OptIn)]
	public partial class TestInfo {
		#region fields
		private int? _Id;
		private int? _F_ShortCode;
		#endregion

		public TestInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Test(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_F_ShortCode == null ? "null" : _F_ShortCode.ToString());
		}
		public static TestInfo Parse(string stringify) {
			if (string.IsNullOrEmpty(stringify) || stringify == "null") return null;
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception($"格式不正确，TestInfo：{stringify}");
			TestInfo item = new TestInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = int.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.F_ShortCode = int.Parse(ret[1]);
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(TestInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("F_ShortCode") ? string.Empty : string.Format(", F_ShortCode : {0}", F_ShortCode == null ? "null" : F_ShortCode.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("F_ShortCode")) ht["F_ShortCode"] = F_ShortCode;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		[JsonProperty] public int? Id {
			get { return _Id; }
			set { _Id = value; }
		}

		[JsonProperty] public int? F_ShortCode {
			get { return _F_ShortCode; }
			set { _F_ShortCode = value; }
		}

		#endregion

		public es.DAL.Test.SqlUpdateBuild UpdateDiy => _Id == null ? null : BLL.Test.UpdateDiy(new List<TestInfo> { this });

		#region sync methods

		public TestInfo Save() {
			if (this.Id != null) {
				if (BLL.Test.Update(this) == 0) return BLL.Test.Insert(this);
				return this;
			}
			return BLL.Test.Insert(this);
		}
		#endregion

		#region async methods

		async public Task<TestInfo> SaveAsync() {
			if (this.Id != null) {
				if (await BLL.Test.UpdateAsync(this) == 0) return await BLL.Test.InsertAsync(this);
				return this;
			}
			return await BLL.Test.InsertAsync(this);
		}
		#endregion
	}
}
