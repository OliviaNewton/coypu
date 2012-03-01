﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Coypu.Drivers;

namespace Coypu.Tests.TestDoubles
{
    public class FakeDriver : Driver
    {
        private readonly IList<Element> clickedElements = new List<Element>();
        private readonly IList<Element> hoveredElements = new List<Element>();
        private readonly IList<Element> checkedElements = new List<Element>();
        private readonly IList<Element> uncheckedElements = new List<Element>();
        private readonly IList<Element> chosenElements = new List<Element>();
        private readonly IList<string> hasContentQueries = new List<string>();
        private readonly IList<Regex> hasContentMatchQueries = new List<Regex>();
        private readonly IList<string> hasCssQueries = new List<string>();
        private readonly IList<string> hasXPathQueries = new List<string>();
        private readonly IList<string> visits = new List<string>();
        private readonly IDictionary<Element, string> setFields = new Dictionary<Element, string>();
        private readonly IDictionary<Element, string> selectedOptions = new Dictionary<Element, string>();
        private readonly Dictionary<string, ElementFound> stubbedButtons = new Dictionary<string, ElementFound>();
        private readonly Dictionary<string, ElementFound> stubbedLinks = new Dictionary<string, ElementFound>();
        private readonly Dictionary<string, ElementFound> stubbedTextFields = new Dictionary<string, ElementFound>();
        private readonly Dictionary<string, ElementFound> stubbedCssResults = new Dictionary<string, ElementFound>();
        private readonly Dictionary<string, ElementFound> stubbedXPathResults = new Dictionary<string, ElementFound>();
        private readonly IDictionary<string, IEnumerable<ElementFound>> stubbedAllCssResults = new Dictionary<string, IEnumerable<ElementFound>>();
        private readonly IDictionary<string, IEnumerable<ElementFound>> stubbedAllXPathResults = new Dictionary<string, IEnumerable<ElementFound>>();
        private readonly IDictionary<string, string> stubbedExecuteScriptResults = new Dictionary<string, string>();
        private readonly IDictionary<string, ElementFound> stubbedFieldsets = new Dictionary<string, ElementFound>();
        private readonly IDictionary<string, ElementFound> stubbedSections = new Dictionary<string, ElementFound>();
        private readonly IDictionary<string, ElementFound> stubbedIFrames = new Dictionary<string, ElementFound>();
        private readonly IDictionary<string, ElementFound> stubbedIDs = new Dictionary<string, ElementFound>();
        private readonly IDictionary<string, bool> stubbedHasContentResults = new Dictionary<string, bool>();
        private readonly IDictionary<Regex, bool> stubbedHasContentMatchResults = new Dictionary<Regex, bool>();
        private readonly IDictionary<string, bool> stubbedHasCssResults = new Dictionary<string, bool>();
        private readonly IDictionary<string, bool> stubbedHasXPathResults = new Dictionary<string, bool>();
        private readonly IDictionary<string, bool> stubbedHasDialogResults = new Dictionary<string, bool>();
        private readonly IList<string> findButtonRequests = new List<string>();
        private readonly IList<string> findLinkRequests = new List<string>();
        private readonly IList<string> findCssRequests = new List<string>();
        private IList<Cookie> stubbedCookies;
        private Uri stubbedLocation;
        private readonly IList<ScopedStubElement> scopedLinks = new List<ScopedStubElement>();

        public FakeDriver() {}
        public FakeDriver(Browser browser)
        {
            Browser = browser;
        }

        public Browser Browser { get; private set; }

        public IEnumerable<Element> ClickedElements
        {
            get { return clickedElements; }
        }

        public IEnumerable<Element> HoveredElements
        {
            get { return hoveredElements; }
        }

        public IDictionary<Element, string> SetFields
        {
            get { return setFields; }
        }

        public IDictionary<Element, string> SelectedOptions
        {
            get { return selectedOptions; }
        }

        public IEnumerable<Element> CheckedElements
        {
            get { return checkedElements; }
        }

        public IEnumerable<Element> ChosenElements
        {
            get { return chosenElements; }
        }

        public IEnumerable<Element> UncheckedElements
        {
            get { return uncheckedElements; }
        }

        public IEnumerable<string> Visits
        {
            get { return visits; }
        }

        public IEnumerable<string> HasContentQueries
        {
            get { return hasContentQueries; }
        }

        public IEnumerable<Regex> HasContentMatchQueries
        {
            get { return hasContentMatchQueries; }
        }

        public IEnumerable<string> HasCssQueries
        {
            get { return hasCssQueries; }
        }

        public IEnumerable<string> HasXPathQueries
        {
            get { return hasXPathQueries; }
        }

        public IEnumerable<string> FindButtonRequests
        {
            get { return findButtonRequests; }
        }

        public IEnumerable<string> FindLinkRequests
        {
            get { return findLinkRequests; }
        }

        public ElementFound FindButton(string locator, DriverScope scope)
        {
            findButtonRequests.Add(locator);
            return stubbedButtons[locator];
        }

        public ElementFound FindLink(string linkText, DriverScope scope)
        {
            findLinkRequests.Add(linkText);

            return stubbedLinks.ContainsKey(linkText) 
                ? stubbedLinks[linkText] 
                : FindScopedElement(scopedLinks, linkText, scope);
        }

        private ElementFound FindScopedElement(IEnumerable<ScopedStubElement> collection, string locator, DriverScope scope)
        {
            var element = collection.FirstOrDefault(scopedLink => scopedLink.Locator == locator && scopedLink.Scope == scope);

            if (element != null)
                return element.Element;

            throw new MissingHtmlException("Element not found: " + locator);
        }

        public ElementFound FindField(string locator, DriverScope scope)
        {
            return stubbedTextFields[locator];
        }

        public void Click(Element element)
        {
            clickedElements.Add(element);
        }

        public void Hover(Element element)
        {
            hoveredElements.Add(element);
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            return stubbedCookies;
        }

        public void SetBrowserCookies(Cookie cookie)
        {
        }

        public void Visit(string url)
        {
            visits.Add(url);
        }

        public void StubButton(string locator, ElementFound element)
        {
            stubbedButtons[locator] = element;
        }

        public void StubLink(string locator, ElementFound element)
        {
            stubbedLinks[locator] = element;
        }

        public void StubField(string locator, ElementFound element)
        {
            stubbedTextFields[locator] = element;
        }

        public void StubHasContent(string text, bool result)
        {
            stubbedHasContentResults.Add(text, result);
        }

        public void StubHasContentMatch(Regex pattern, bool result)
        {
            stubbedHasContentMatchResults.Add(pattern, result);
        }

        public void StubHasCss(string cssSelector, bool result)
        {
            stubbedHasCssResults.Add(cssSelector, result);
        }

        public void StubHasXPath(string xpath, bool result)
        {
            stubbedHasXPathResults.Add(xpath, result);
        }

        public void StubDialog(string text, bool result)
        {
            stubbedHasDialogResults.Add(text, result);
        }

        public void StubCss(string cssSelector, ElementFound result)
        {
            stubbedCssResults.Add(cssSelector, result);
        }

        public void StubXPath(string cssSelector, ElementFound result)
        {
            stubbedXPathResults.Add(cssSelector, result);
        }

        public void StubAllCss(string cssSelector, IEnumerable<ElementFound> result)
        {
            stubbedAllCssResults.Add(cssSelector, result);
        }

        public void StubAllXPath(string xpath, IEnumerable<ElementFound> result)
        {
            stubbedAllXPathResults.Add(xpath, result);
        }

        public void Dispose()
        {
            Disposed = true;
        }

        public bool Disposed { get; private set; }

        public Uri Location
        {
            get { return stubbedLocation; }
        }

        public ElementFound Window
        {
            get { throw new NotImplementedException(); }
        }

        public void AcceptModalDialog()
        {
            ModalDialogsAccepted++;
        }

        public void CancelModalDialog()
        {
            ModalDialogsCancelled++;
        }

        private ElementFound Find(IDictionary<string, ElementFound> dictionary, string locator)
        {
            if (dictionary.ContainsKey(locator))
                return dictionary[locator];

            throw new MissingHtmlException("Element not found: " + locator);
        }

        public string ExecuteScript(string javascript)
        {
            return stubbedExecuteScriptResults[javascript];
        }

        public ElementFound FindFieldset(string locator, DriverScope scope)
        {
            return Find(stubbedFieldsets,locator);
        }

        public ElementFound FindSection(string locator, DriverScope scope)
        {
            return Find(stubbedSections,locator);
        }

        public ElementFound FindId(string id, DriverScope scope)
        {
            return Find(stubbedIDs,id);
        }

        public ElementFound FindIFrame(string locator, DriverScope scope)
        {
            return Find(stubbedIFrames,locator);
        }

        public void Set(Element element, string value)
        {
            setFields.Add(element, value);
        }

        public void Select(Element element, string option)
        {
            selectedOptions.Add(element, option);
        }

        public object Native
        {
            get { return "Native driver on fake driver"; }
        }

        public int ModalDialogsAccepted { get; private set; }

        public int ModalDialogsCancelled { get; private set; }

        public IList<string> FindCssRequests
        {
            get { return findCssRequests; }
        }

        public bool HasContent(string text, DriverScope scope)
        {
            hasContentQueries.Add(text);
            return stubbedHasContentResults[text];
        }

        public bool HasContentMatch(Regex pattern, DriverScope scope)
        {
            hasContentMatchQueries.Add(pattern);
            return stubbedHasContentMatchResults[pattern];
        }

        public bool HasCss(string cssSelector, DriverScope scope)
        {
            return stubbedHasCssResults[cssSelector];
        }

        public bool HasXPath(string xpath, DriverScope scope)
        {
            return stubbedHasXPathResults[xpath];
        }

        public bool HasDialog(string withText)
        {
            return stubbedHasDialogResults[withText];
        }

        public ElementFound FindCss(string cssSelector, DriverScope scope)
        {
            FindCssRequests.Add(cssSelector);
            return Find(stubbedCssResults,cssSelector);
        }

        public ElementFound FindXPath(string xpath, DriverScope scope)
        {
            return Find(stubbedXPathResults,xpath);
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector, DriverScope scope)
        {
            return stubbedAllCssResults[cssSelector];
        }

        public IEnumerable<ElementFound> FindAllXPath(string xpath, DriverScope scope)
        {
            return stubbedAllXPathResults[xpath];
        }

        public void Check(Element field)
        {
            checkedElements.Add(field);
        }

        public void Uncheck(Element field)
        {
            uncheckedElements.Add(field);
        }

        public void Choose(Element field)
        {
            chosenElements.Add(field);
        }

        public void StubExecuteScript(string script, string scriptReturnValue)
        {
            stubbedExecuteScriptResults.Add(script, scriptReturnValue);
        }

        public void StubFieldset(string locator, ElementFound fieldset)
        {
            stubbedFieldsets.Add(locator, fieldset);
        }
        
        public void StubSection(string locator, ElementFound section)
        {
            stubbedSections.Add(locator, section);
        }

        public void StubIFrame(string locator, ElementFound iframe)
        {
            stubbedIFrames.Add(locator, iframe);
        }

        public void StubId(string id, ElementFound element)
        {
            stubbedIDs.Add(id, element);
        }

        public void StubCookies(List<Cookie> cookies)
        {
            stubbedCookies = cookies;
        }

        public void StubLocation(Uri location)
        {
            stubbedLocation = location;
        }

        public void StubLink(string locator, StubElement element, DriverScope scope)
        {
            scopedLinks.Add(new ScopedStubElement {Locator = locator, Element = element, Scope = scope});
        }
    }

    class ScopedStubElement
    {
        public string Locator;
        public StubElement Element;
        public DriverScope Scope;
    }
}