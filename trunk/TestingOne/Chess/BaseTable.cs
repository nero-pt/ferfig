/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

using System;
using System.Collections;

namespace Chess
{
	public class BaseTable : IEnumerable
	{
		private Hashtable mTable;

		public BaseTable()
		{
			mTable=new Hashtable();
		}

		public bool Contains(string key)
		{
			return mTable.ContainsKey(key);
		}

		protected bool Remove(string key)
		{
			bool exists=Contains(key);
			mTable.Remove(key);
			return exists;
		}

		protected void Add(string key,object val)
		{
			if (this.Contains(key))
			{
				this.Remove(key);
			}
			mTable.Add(key,val);		
		}

		public int Count
		{
			get{return mTable.Count;}
		}

		public IEnumerator GetEnumerator()
		{
			return new ValueEnumerator(mTable);
		}

		protected object Get(string name)
		{
			return mTable[name];
		}

		public override string ToString()
		{
			string s="";

			foreach (object o in this)
			{
				s+=o.ToString() + "\n";
			}

			return s;
		}

		protected void ClearTable()
		{
			mTable.Clear();
		}
	}

	public class ValueEnumerator : IEnumerator
	{
		IEnumerator mHashEnum;

		public ValueEnumerator(Hashtable table)
		{
			mHashEnum=table.GetEnumerator();	
		}

		public void Reset()
		{
			mHashEnum.Reset();
		}

		public object Current
		{
			get
			{
				DictionaryEntry item=(DictionaryEntry) mHashEnum.Current;
				return item.Value;
			}
		}

		public bool MoveNext()
		{
			return mHashEnum.MoveNext();
		}
	}
}
