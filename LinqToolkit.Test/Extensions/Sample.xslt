<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
  exclude-result-prefixes="msxsl"
  >
  <xsl:output omit-xml-declaration="yes" method="text" indent="no" />

  <xsl:template match="/" ><xsl:apply-templates select="./Options" /></xsl:template>
  <xsl:variable name="QueryPart" />
  <xsl:template match="Options">SELECT<xsl:call-template name="BeforeSelect" /><xsl:apply-templates select="./PropertiesToRead" />
<xsl:call-template name="AfterSelect" />
FROM <xsl:value-of select="./Source"/><xsl:call-template name="AfterFrom" />
WHERE <xsl:apply-templates select="./Filter" /><xsl:call-template name="AfterWhere" />
  </xsl:template>

  <xsl:template match="PropertiesToRead"><xsl:for-each select="./Property" >
      <xsl:if test="position()>1">,</xsl:if>
      <xsl:text> </xsl:text>
      <xsl:value-of select="."/>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="BeforeSelect">
    <xsl:for-each select="./Operators/Operator">
      <xsl:choose>
        <xsl:when test="./@OperatorName='Distinct'"> DISTINCT</xsl:when>
        <xsl:when test="./@OperatorName='Take'"> TOP <xsl:apply-templates select="./Value"/></xsl:when>
      </xsl:choose>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="AfterSelect"></xsl:template>

  <xsl:template name="AfterFrom">
    <xsl:for-each select="./Operators/Operator">
    <xsl:choose>
      <xsl:when test="./@OperatorName='OrderBy'">
ORDER BY <xsl:value-of select="@PropertyName" /></xsl:when>
      <xsl:when test="./@OperatorName='OrderByDescending'">
ORDER BY <xsl:value-of select="@PropertyName" /> DESCENDING</xsl:when>
      <xsl:when test="./@OperatorName='ThenBy'">, <xsl:value-of select="@PropertyName" /></xsl:when>
      <xsl:when test="./@OperatorName='ThenByDescending'">, <xsl:value-of select="@PropertyName" /> DESCENDING</xsl:when>
    </xsl:choose>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="AfterWhere"></xsl:template>

  <xsl:template match="node()[@xsi:type='JoinOperation']" >(<xsl:apply-templates select="./Left"/><xsl:apply-templates select="./@Type" /><xsl:apply-templates select="./Right"/>)</xsl:template>
  <xsl:template match="node()[@xsi:type='UnaryOperation']" >(<xsl:apply-templates select="./@Type" /><xsl:value-of select="@PropertyName"/>)</xsl:template>
  <xsl:template match="node()[@xsi:type='BinaryOperation']" >(<xsl:value-of select="@PropertyName"/><xsl:apply-templates select="./@Type" /><xsl:apply-templates select="./Value"/>)</xsl:template>
  <xsl:template match="node()[@xsi:type='CallOperation']" >(<xsl:value-of select="@PropertyName"/>.<xsl:value-of select="@MethodName"/><xsl:apply-templates select="./Arguments"/>)</xsl:template>

  <xsl:template match="Arguments">(<xsl:for-each select="./Value" >
      <xsl:if test="position()>1">, </xsl:if>
      <xsl:apply-templates select="."/>
    </xsl:for-each>)</xsl:template>

  <xsl:template name="Type">
    <xsl:choose >
      <xsl:when test="./@Type='Add'"> Add </xsl:when>
      <xsl:when test="./@Type='AddChecked'"> AddChecked </xsl:when>
      <xsl:when test="./@Type='And'"> And </xsl:when>
      <xsl:when test="./@Type='AndAlso'"> AndAlso </xsl:when>
      <xsl:when test="./@Type='ArrayLength'"> ArrayLength </xsl:when>
      <xsl:when test="./@Type='ArrayIndex'"> ArrayIndex </xsl:when>
      <xsl:when test="./@Type='Call'"> Call </xsl:when>
      <xsl:when test="./@Type='Coalesce'"> Coalesce </xsl:when>
      <xsl:when test="./@Type='Conditional'"> Conditional </xsl:when>
      <xsl:when test="./@Type='Constant'"> Constant </xsl:when>
      <xsl:when test="./@Type='Convert'"> Convert </xsl:when>
      <xsl:when test="./@Type='ConvertChecked'"> ConvertChecked </xsl:when>
      <xsl:when test="./@Type='Divide'"> Divide </xsl:when>
      <xsl:when test="./@Type='Equal'"> Equal </xsl:when>
      <xsl:when test="./@Type='ExclusiveOr'"> ExclusiveOr </xsl:when>
      <xsl:when test="./@Type='GreaterThan'"> GreaterThan </xsl:when>
      <xsl:when test="./@Type='GreaterThanOrEqual'"> GreaterThanOrEqual </xsl:when>
      <xsl:when test="./@Type='Invoke'"> Invoke </xsl:when>
      <xsl:when test="./@Type='Lambda'"> Lambda </xsl:when>
      <xsl:when test="./@Type='LeftShift'"> LeftShift </xsl:when>
      <xsl:when test="./@Type='LessThan'"> LessThan </xsl:when>
      <xsl:when test="./@Type='LessThanOrEqual'"> LessThanOrEqual </xsl:when>
      <xsl:when test="./@Type='ListInit'"> ListInit </xsl:when>
      <xsl:when test="./@Type='MemberAccess'"> MemberAccess </xsl:when>
      <xsl:when test="./@Type='MemberInit'"> MemberInit </xsl:when>
      <xsl:when test="./@Type='Modulo'"> Modulo </xsl:when>
      <xsl:when test="./@Type='Multiply'"> Multiply </xsl:when>
      <xsl:when test="./@Type='MultiplyChecked'"> MultiplyChecked </xsl:when>
      <xsl:when test="./@Type='Negate'"> Negate </xsl:when>
      <xsl:when test="./@Type='UnaryPlus'"> UnaryPlus </xsl:when>
      <xsl:when test="./@Type='NegateChecked'"> NegateChecked </xsl:when>
      <xsl:when test="./@Type='New'"> New </xsl:when>
      <xsl:when test="./@Type='NewArrayInit'"> NewArrayInit </xsl:when>
      <xsl:when test="./@Type='NewArrayBounds'"> NewArrayBounds </xsl:when>
      <xsl:when test="./@Type='Not'"> Not </xsl:when>
      <xsl:when test="./@Type='NotEqual'"> NotEqual </xsl:when>
      <xsl:when test="./@Type='Or'"> Or </xsl:when>
      <xsl:when test="./@Type='OrElse'"> OrElse </xsl:when>
      <xsl:when test="./@Type='Parameter'"> Parameter </xsl:when>
      <xsl:when test="./@Type='Power'"> Power </xsl:when>
      <xsl:when test="./@Type='Quote'"> Quote </xsl:when>
      <xsl:when test="./@Type='RightShift'"> RightShift </xsl:when>
      <xsl:when test="./@Type='Subtract'"> Subtract </xsl:when>
      <xsl:when test="./@Type='SubtractChecked'"> SubtractChecked </xsl:when>
      <xsl:when test="./@Type='TypeAs'"> TypeAs </xsl:when>
      <xsl:when test="./@Type='TypeIs'"> TypeIs </xsl:when>
      <xsl:otherwise>
        <xsl:message terminate="yes">Type <xsl:value-of select="./@Type"/> not supported.</xsl:message>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template match="Value">
    <xsl:choose>
      <xsl:when test="./@xsi:type='xsd:int'"><xsl:value-of select="."/></xsl:when>
      <xsl:when test="./@xsi:type='xsd:string'">"<xsl:value-of select="."/>"</xsl:when>
      <xsl:when test="./@xsi:type='xsd:bool'"><xsl:value-of select="."/></xsl:when>
      <xsl:when test="./@xsi:type='xsd:decimal'"><xsl:value-of select="."/></xsl:when>
      <xsl:otherwise>
        <xsl:message terminate="yes">Value of type <xsl:value-of select="./@xsi:type"/> not supported.</xsl:message>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>
