using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace LinqToolkit.Test {
    public class TestItem: ITestItem {

        private string testPropertySimple;
        private int testPropertyWithSourceProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool TestField;

        public string TestPropertySimple {
            get { return this.testPropertySimple; }
            set {
                this.testPropertySimple = value;
                this.OnPropertyChanged( "TestPropertySimple" );
            }
        }
        [SourceProperty( "TestSourceProperty" )]
        public int TestPropertyWithSourceProperty {
            get { return this.testPropertyWithSourceProperty; }
            set {
                this.testPropertyWithSourceProperty = value;
                this.OnPropertyChanged( "TestPropertyWithSourceProperty" );
            }
        }
        [Ignore]
        public string TestPropertyWithIgnore { get; set; }

        protected void OnPropertyChanged( string propertyName ) {
            if ( this.PropertyChanged!=null ) {
                this.PropertyChanged(
                    this,
                    new PropertyChangedEventArgs( propertyName )
                    );
            }
        }
    }
}
