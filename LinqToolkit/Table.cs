using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using LinqToolkit.Properties;

namespace LinqToolkit {

    [Serializable]
    public abstract class Table<TItem>: IEnumerable<TItem>
        where TItem: INotifyPropertyChanged {
        #region private static fields
        private static PropertyInfo[] itemProperties;
        #endregion private static fields
        #region private fields
        private List<TItem> items;
        private HashSet<TItem> inserted;
        private HashSet<TItem> deleted;
        private Dictionary<TItem, HashSet<string>> updated;
        private PropertyChangedEventHandler itemPropertyChangedEventHandler;
        #endregion private fields
        #region public properties
        public IEnumerable<TItem> Inserted {
            get { return this.inserted; }
        }
        public IEnumerable<TItem> Deleted {
            get { return this.deleted; }
        }
        public IEnumerable<UpdatedTableItem<TItem>> Updated {
            get {
                return
                    from item in this.updated
                    select new UpdatedTableItem<TItem>( item.Key, item.Value );
            }
        }
        #endregion public properties
        #region constructors
        static Table() {
            itemProperties = typeof( TItem ).GetProperties();
        }
        public Table( IEnumerable<TItem> items ) {
            if ( items == null ) {
                throw new ArgumentNullException(
                    "items",
                    Resources.TableConstructorItemsArgumentNullException
                    );
            }
            this.items = items.ToList();
            this.inserted = new HashSet<TItem>();
            this.deleted = new HashSet<TItem>();
            this.updated = new Dictionary<TItem, HashSet<string>>();
            this.itemPropertyChangedEventHandler = new PropertyChangedEventHandler( this.ItemPropertyChanged );
            foreach ( var item in this.items ) {
                item.PropertyChanged += this.itemPropertyChangedEventHandler;
            }
        }
        #endregion constructors
        #region public methods
        public void Insert( TItem item ) {
            if ( item == null ) {
                throw new ArgumentNullException( "item", Resources.TableInsertItemNull );
            }
            if ( this.deleted.Contains( item ) ) {
                throw new ArgumentException( Resources.TableInsertItemAlreadyDeleted, "item" );
            }
            if ( this.items.Contains( item ) ) {
                throw new ArgumentException( Resources.TableInsertItemAlreadyExists, "item" );
            }
            this.items.Add( item );
            this.inserted.Add( item );
        }
        public void Delete( TItem item ) {
            if ( item == null ) {
                throw new ArgumentNullException( "item", Resources.TableDeleteItemNull );
            }
            if ( !this.items.Remove( item ) ) {
                throw new ArgumentException( Resources.TableDeleteItemNotFound, "item" );
            }
            if ( this.inserted.Remove( item ) ) {
                return;
            }
            this.updated.Remove( item );
            this.deleted.Add( item );
            item.PropertyChanged -= this.itemPropertyChangedEventHandler;
        }
        #endregion public methods
        #region abstract methods
        public abstract void SubmitChanges();
        #endregion abstract methods
        #region event handlers
        private void ItemPropertyChanged( object sender, PropertyChangedEventArgs e ) {
            var item = (TItem)sender;
            if ( !this.updated.ContainsKey( item ) ) {
                this.updated.Add( item, new HashSet<string>() );
            }
            this.updated[item].Add( e.PropertyName );
        }
        #endregion event handlers
        #region IEnumerable<TItem> Members
        public IEnumerator<TItem> GetEnumerator() {
            return this.items.GetEnumerator();
        }
        #endregion
        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
        #endregion
    }
}
