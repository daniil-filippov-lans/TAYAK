namespace eMark;

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

class Block : Element
{
	public Block(Element parent, XmlElement xml) : base(parent, xml) { }
	public Block() : base() { }
}
