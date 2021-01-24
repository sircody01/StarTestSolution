using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using OpenQA.Selenium;
using Star.Core.Utilities;

namespace Star.Core.Extensions
{
    /// <summary>
    /// Adds additional assertion types to the FluentAssertion Should() construct.
    /// </summary>
    [DebuggerNonUserCode]
    public static partial class AssertionExtensions
    {
        public static WebDriverAssertions Should(this IWebDriver webDriver)
        {
            return new WebDriverAssertions(webDriver);
        }

        public static WebElementAssertions Should(this IWebElement element)
        {
            return new WebElementAssertions(element);
        }

        public static WebElementListAssertions Should(this IEnumerable<IWebElement> elementList)
        {
            return new WebElementListAssertions(elementList);
        }
    }

    /// <summary>
    /// Custom assertion class for IWebElement specific assertions.
    /// </summary>
    [DebuggerNonUserCode]
    public class WebElementAssertions : ReferenceTypeAssertions<IWebElement, WebElementAssertions>
    {
        protected override string Identifier => "IWebElement";

        public WebElementAssertions(IWebElement value)
        {
            Subject = value;
        }

        /// <summary>
        /// Asserts that the element is currently displayed.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> BeDisplayed(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Displayed)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected the element to be displayed{reason}, but found it not displayed.");

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the elements Displayed status matches the expected value.
        /// </summary>
        /// <param name="expected">
        /// The expected Displayed state to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> BeDisplayed(bool expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Displayed.Equals(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to be {0}{reason}, but found it is {1}.",
                    expected ? "displayed" : "not dispalyed", expected ? "not displayed" : "displayed");

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element is currently not displayed.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> NotBeDisplayed(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.Displayed)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to not be dispalyed{reason}, but found it is displayed.");

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element is currently visible.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> BeVisible(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.IsVisible())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to be visible{0}{reason}, but found it not visible.");

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the elements IsVisible state matches the expected value.
        /// </summary>
        /// <param name="expected">
        /// The expected IsVisible state to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> BeVisible(bool expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.IsVisible().Equals(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to be {0}{reason}, but found it is {1}.",
                    expected ? "visible" : "not visible", expected ? "not visible" : "visible");

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element is currently not visible.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> NotBeVisible(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.IsVisible())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to not be visible{reason}, but found it is visible.");

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element has the specified class applied to it.
        /// </summary>
        /// <param name="className">
        /// The name of the class to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> HaveClass(string className, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasClass(className))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to have class {0}{reason}, but found it has class {1}.", className, Subject.GetAttribute("class"));

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element HasClass status for the specified class matches the expected value.
        /// </summary>
        /// <param name="className">
        /// The class name to look for.
        /// </param>
        /// <param name="shouldExist">
        /// The expected HasClass state to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> HaveClass(string className, bool shouldExist, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasClass(className).Equals(shouldExist))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected the class name {0} to {1}{reason}, but found it has class name {2}.", className,
                    shouldExist ? "exist" : "not exist", Subject.GetAttribute("class"));

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element does not have the specified class applied to it.
        /// </summary>
        /// <param name="className">
        /// The name of the class to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> NotHaveClass(string className, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasClass(className))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to not have class name {0}{reason}, but found it has the class {1}.", className,
                    Subject.GetAttribute("class"));

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the text of the element matches the expected value exactly.
        /// </summary>
        /// <param name="expectedText">
        /// The text to match exactly.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> Be(string expectedText, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Text.Equals(expectedText))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element text to be {0}{reason}, but found {1}.", expectedText, Subject.Text);

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the price string contained in the element matches the specified value.
        /// </summary>
        /// <param name="expectedValue">
        /// The expected decimal value.
        /// </param>
        /// <param name="countryCode">
        /// The country code to use when converting the elements text to decimal.
        /// </param>
        /// <param name="because"></param>
        /// <param name="becauseArgs"></param>
        public AndConstraint<WebElementAssertions> Be(decimal expectedValue, string countryCode, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Text.PriceStringToDecimal(countryCode).Equals(expectedValue))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected price string to be equal to {0}{reason}, but found {1}.", expectedValue, Subject.Text);

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="string"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="attributeName">
        /// The name of the attribute of the element to check the value of.
        /// </param>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> HaveAttributeValueOneOf(string attributeName, string[] validValues,
            string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(validValues.Contains(Subject.GetAttribute(attributeName)))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected the element's attribute value to be one of {0}{reason}, but found {1}.",
                    validValues, Subject.GetAttribute(attributeName));

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the text of the element contains the expected text, including matching case.
        /// </summary>
        /// <param name="expectedText">
        /// The text to look for with matching case.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> ContainText(string expectedText, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Text.Contains(expectedText))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element text to contain {0}{reason}, but found {1}.", expectedText, Subject.Text);

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element has the specified value.
        /// </summary>
        /// <param name="expectedValue">
        /// Value of the element to compare.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> HaveValue(string expectedValue, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.GetAttribute("value").Equals(expectedValue))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element's value to be {0}{reason}, but found the value to be {1}.", expectedValue,
                    Subject.GetAttribute("value"));

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the href attribute of the element has/does not have the specified value.
        /// </summary>
        /// <param name="attribueValue">
        /// The value of the href attribute to compare.
        /// </param>
        /// <param name="expected">
        /// Whether the value should be present or absent.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> HaveHrefContaining(string attribueValue, bool expected,
            string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.GetAttribute("href").Contains(attribueValue).Equals(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected href value to {1} {0}{reason}, but found href value {2}.", attribueValue,
                    expected ? "contain" : "not contain", Subject.GetAttribute("href"));

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the specified CSS attribute has the specified value using Contains.
        /// </summary>
        /// <param name="cssAttribute">
        /// The CSS attribute to test.
        /// </param>
        /// <param name="cssValue">
        /// The CSS value to compare.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> ContainCssValue(string cssAttribute, string cssValue,
            string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.GetCssValue(cssAttribute).Contains(cssValue))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected CSS attribute {0} to have value {1}{reason}, but found CSS value {2}.", cssAttribute,
                    cssValue, Subject.GetCssValue(cssAttribute));

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the src attribute equals the specified value.
        /// </summary>
        /// <param name="value">
        /// The value to compare.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> HaveSrcValue(string value, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.GetAttribute("src").Equals(value))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected src attribute to have value {0}{reason}, but found value {1}.", value,
                    Subject.GetAttribute("src"));

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the elements text contains all the specified values.
        /// </summary>
        /// <param name="values">
        /// The list of values to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> ContainsAll(string[] values, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(values.All(value => Subject.Text.Contains(value)))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element text {0} to have all the values{reason}, but the value {1} is missing.",
                    Subject.Text, values.FirstOrDefault(value => !Subject.Text.Contains(value)));

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the elements text contains any of the specified values.
        /// </summary>
        /// <param name="values">
        /// The list of values to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> ContainsAny(string[] values, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(values.Any(value => Subject.Text.Contains(value)))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element text to containe one of {0}{reason}, but found {1}.",
                    values, Subject.Text);

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element has the specified attribute with the specified value.
        /// </summary>
        /// <param name="attribute">
        /// The attribute to look for.
        /// </param>
        /// <param name="value">
        /// The value of the attribute to compare.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> HaveAttributeValue(string attribute, string value, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.GetAttribute(attribute).Equals(value))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to have the attribute {0} with value {1){reason}, but found value to be {2}.",
                    attribute, value, Subject.GetAttribute(attribute));

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element not has the specified attribute with the specified value.
        /// </summary>
        /// <param name="attribute">
        /// The attribute to look for.
        /// </param>
        /// <param name="value">
        /// The value of the attribute to compare.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> NotHaveAttributeValue(string attribute, string value, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.GetAttribute(attribute).Equals(value))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element not to have the attribute {0} with value {1){reason}, but found value to be {2}.",
                    attribute, value, Subject.GetAttribute(attribute));

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element has the specified attribute applied to it.
        /// </summary>
        /// <param name="attributeName">
        /// The name of the attribute to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> HaveAttribute(string attributeName, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(ElementHasAttribute(Subject, attributeName))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to have attribute {0}{reason}, but found it does not have the attribute {0}.", attributeName);

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element does not have the specified attribute applied to it.
        /// </summary>
        /// <param name="attributeName">
        /// The name of the attribute to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> NotHaveAttribute(string attributeName, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!ElementHasAttribute(Subject, attributeName))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected element to not have attribute {0}{reason}, but found the element has the attribute {0}.",
                    attributeName);

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Assert that the element is enabled.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> BeEnabled(string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Enabled)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to be enabled{reason}, but found it disabled.");

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Assert that the element is not enabled.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> NotBeEnabled(string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.Enabled)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected to be disabled{reason}, but found it enabled.");

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Assert that the element has the specified 'child' element.
        /// </summary>
        /// <param name="bySelector">The Selenium find by parameters to use when searching for the element.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> HaveElement(By bySelector, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.ElementExists(bySelector))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected child element {0} to exist{reason}, but it does not exist.", bySelector.ToString());

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Assert that the element has the specified 'child' element.
        /// </summary>
        /// <param name="bySelector">The Selenium find by parameters to use when searching for the element.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> NotHaveElement(By bySelector, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.ElementExists(bySelector))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected child element {0} to not exist{reason}, but it does exist.", bySelector.ToString());

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element is currently selected.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> BeSelected(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Selected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to be selected{reason}, but it is not.");

            return new AndConstraint<WebElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the element is currently not selected.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementAssertions> NotBeSelected(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.Selected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to not be selected{reason}, but it is selected.");

            return new AndConstraint<WebElementAssertions>(this);
        }

        private static bool ElementHasAttribute(IWebElement element, string attribute)
        {
            return element.GetAttribute(attribute) != null;
        }
    }

    /// <summary>
    /// Custom assertion class for IEnumerable&lt;IWebElement&gt; specific assertions.
    /// </summary>
    [DebuggerNonUserCode]
    public class WebElementListAssertions : GenericCollectionAssertions<IWebElement>
    {
        public WebElementListAssertions(IEnumerable<IWebElement> elementList)
            : base(elementList)
        {
        }

        /// <summary>
        /// Asserts that all elements are Displayed.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementListAssertions> AllBeDisplayed(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.All(webElement => webElement.Displayed))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all elements to be displayed{reason}, but found {0} elements not displayed.",
                    Subject.Count(webElement => !webElement.Displayed));

            return new AndConstraint<WebElementListAssertions>(this);
        }

        /// <summary>
        /// Asserts that all elements are not Displayed.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementListAssertions> AllBeNotDisplayed(string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.All(webElement => !webElement.Displayed))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all elements to not be displayed{reason}, but found {0} displayed elements.",
                    Subject.Count(webElement => webElement.Displayed));

            return new AndConstraint<WebElementListAssertions>(this);
        }

        /// <summary>
        /// Asserts that all elements are Visible.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementListAssertions> AllBeVisible(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.All(webElement => webElement.IsVisible()))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all elements to be visible{reason}, but found {0} elements that are not visible.",
                    Subject.Count(webElement => !webElement.IsVisible()));

            return new AndConstraint<WebElementListAssertions>(this);
        }

        /// <summary>
        /// Asserts that all elements are not Visible.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementListAssertions> AllBeNotVisible(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.All(webElement => !webElement.IsVisible()))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all elements to not be visible{reason}, but found {0} elements that are visible.",
                    Subject.Count(webElement => webElement.IsVisible()));

            return new AndConstraint<WebElementListAssertions>(this);
        }

        /// <summary>
        /// Asserts that all elements have the specified class name.
        /// </summary>
        /// <param name="className">
        /// The class name to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementListAssertions> AllHaveClass(string className, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.All(webElement => webElement.HasClass(className)))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected all elements to have class name {0}{reason}, but found {1} elements that do not have the class name.",
                    className, Subject.Count(webElement => !webElement.HasClass(className)));

            return new AndConstraint<WebElementListAssertions>(this);
        }

        /// <summary>
        /// Asserts that all elements do not have the specified class name.
        /// </summary>
        /// <param name="className">
        /// The class name to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementListAssertions> AllNotHaveClass(string className, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.All(webElement => !webElement.HasClass(className)))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected all elements to not have class name {0}{reason}, but found {1} elements with the class name.",
                    className, Subject.Count(webElement => webElement.HasClass(className)));

            return new AndConstraint<WebElementListAssertions>(this);
        }

        /// <summary>
        /// Asserts that the specified element text can be found in the element list using Contains.
        /// </summary>
        /// <param name="elementText">
        /// The element text to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementListAssertions> HaveElementTextContaining(string elementText, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Any(webElement => webElement.Text.Contains(elementText)))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected an element to have the text {0}{reason}, but found no element with the text.",
                    elementText);

            return new AndConstraint<WebElementListAssertions>(this);
        }

        /// <summary>
        /// Asserts that the specified element text can/cannot be found in the element list using Contains.
        /// </summary>
        /// <param name="elementText">
        /// The element text to look for.
        /// </param>
        /// <param name="expected">
        /// Whether the text should or should not exist.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementListAssertions> HaveElementTextContaining(string elementText, bool expected,
            string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Any(webElement => webElement.Text.Contains(elementText)).Equals(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {1} to have the text {0}{reason}, but found {2} with the text.",
                    elementText, expected ? "an element" : "no element", expected ? "no element" : "an element");

            return new AndConstraint<WebElementListAssertions>(this);
        }

        /// <summary>
        /// Asserts that the specified element text cannot be found in the element list using Contains.
        /// </summary>
        /// <param name="elementText">
        /// The element text to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementListAssertions> NotHaveElementTextContaining(string elementText,
            string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.Any(webElement => webElement.Text.Contains(elementText)))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected no element to have the text {0}{reason}, but found an element with the text {1}.",
                    elementText, Subject.FirstOrDefault(webElement => webElement.Text.Contains(elementText)).Text);

            return new AndConstraint<WebElementListAssertions>(this);
        }

        /// <summary>
        /// Asserts that the specified element text can be found in the element list using Equals.
        /// </summary>
        /// <param name="elementText">
        /// The element text to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementListAssertions> HaveElementTextEqualTo(string elementText, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Any(webElement => webElement.Text.Equals(elementText)))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected an element to have the text {0}{reason}, but found no element with the text.",
                    elementText);

            return new AndConstraint<WebElementListAssertions>(this);
        }

        /// <summary>
        /// Asserts that the specified element text can/cannot be found in the element list using Equals.
        /// </summary>
        /// <param name="elementText">
        /// The element text to look for.
        /// </param>
        /// <param name="expected">
        /// Whether the text should or should not exist.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementListAssertions> HaveElementTextEqualTo(string elementText, bool expected,
            string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Any(webElement => webElement.Text.Equals(elementText)).Equals(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected to find {1} element's text to equal the text {0}{reason}, but found {2} element with the text.",
                    elementText, expected ? "an" : "no", expected ? "no" : "an");

            return new AndConstraint<WebElementListAssertions>(this);
        }

        /// <summary>
        /// Asserts that the specified element text can be found in the element list using Equals.
        /// </summary>
        /// <param name="elementText">
        /// The element text to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebElementListAssertions> HaveNoElementTextEqualTo(string elementText, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.Any(webElement => webElement.Text.Equals(elementText)))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected no element to have the text {0}{reason}, but at least one was found.", elementText);

            return new AndConstraint<WebElementListAssertions>(this);
        }
    }

    /// <summary>
    /// Custom assertion class for WebDriver specific assertions.
    /// </summary>
    [DebuggerNonUserCode]
    public class WebDriverAssertions : ReferenceTypeAssertions<IWebDriver, WebDriverAssertions>
    {
        protected override string Identifier => "IWebDriver";

        public WebDriverAssertions(IWebDriver webDriver)
        {
            Subject = webDriver;
        }

        /// <summary>
        /// Asserts that a specified element exists in the browsers DOM.
        /// </summary>
        /// <param name="bySelector">The Selenium find by parameters to use when searching for the element.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebDriverAssertions> HaveElement(By bySelector, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.ElementExists(bySelector))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element {0} to exist{reason}, but it does not exist.", bySelector.ToString());

            return new AndConstraint<WebDriverAssertions>(this);
        }

        /// <summary>
        /// Asserts that a specified element does/does not exist in the browsers DOM.
        /// </summary>
        /// <param name="bySelector">The Selenium find by parameters to use when searching for the element.</param>
        /// <param name="expected">
        /// The expected Displayed state to look for.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebDriverAssertions> HaveElement(By bySelector, bool expected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.ElementExists(bySelector).Equals(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element {0} to {1}{reason}, but it does {2}.", bySelector.ToString(),
                    expected ? "exist" : "not exist", expected ? "not exist" : "exist");

            return new AndConstraint<WebDriverAssertions>(this);
        }

        /// <summary>
        /// Asserts that a specified element does not exist in the browsers notDOM.
        /// </summary>
        /// <param name="bySelector">The Selenium find by parameters to use when searching for the element.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<WebDriverAssertions> NotHaveElement(By bySelector, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.ElementExists(bySelector))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element {0} to not exist{reason}, but it does exist.", bySelector.ToString());

            return new AndConstraint<WebDriverAssertions>(this);
        }
    }
}
