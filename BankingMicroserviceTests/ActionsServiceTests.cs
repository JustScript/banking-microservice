using BankingMicroservice.Models;
using BankingMicroservice.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankingMicroserviceTests
{
    public class ActionsServiceTests
    {
        public static IEnumerable<object[]> PDFExamplesTestDataSet
        {
            get
            {
                // Dla karty PREPAID w statusie CLOSED aplikacja powinna zwrócić akcje: ACTION3, ACTION4, ACTION9.
                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Closed, true),
                    new List<string>(){ "ACTION3", "ACTION4", "ACTION9" }
                };

                // Dla karty CREDIT w statusie BLOCKED aplikacja powinna zwrócić akcje: ACTION3, ACTION4, ACTION5, ACTION6 (jeżeli pin nadany), ACTION7 (jeżeli pin nadany), ACTION8, ACTION9
                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Blocked, true), // With PIN
                    new List<string>(){ "ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION7", "ACTION8", "ACTION9" }
                };

                // Dla karty CREDIT w statusie BLOCKED aplikacja powinna zwrócić akcje: ACTION3, ACTION4, ACTION5, ACTION6 (jeżeli pin nadany), ACTION7 (jeżeli pin nadany), ACTION8, ACTION9
                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Blocked, false), // Without PIN
                    new List<string>(){ "ACTION3", "ACTION4", "ACTION5", "ACTION8", "ACTION9" }
                };
            }
        }

        public static IEnumerable<object[]> PrepaidWithPINTestDataSet
        {
            get
            {
                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Ordered, true),
                    new List<string>(){ "ACTION3", "ACTION4", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION12", "ACTION13" }
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Inactive, true),
                    new List<string>(){ "ACTION2","ACTION3","ACTION4","ACTION6","ACTION8","ACTION9","ACTION10","ACTION11","ACTION12","ACTION13" }
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Active, true),
                    new List<string>(){"ACTION1", "ACTION3", "ACTION4", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Restricted, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Blocked, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION6", "ACTION7", "ACTION8", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Expired, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Closed, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION9"}
                };
            }
        }

        public static IEnumerable<object[]> DebitWithPINTestDataSet
        {
            get
            {
                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Ordered, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Inactive, true),
                    new List<string>(){"ACTION2", "ACTION3", "ACTION4", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Active, true),
                    new List<string>(){"ACTION1", "ACTION3", "ACTION4", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Restricted, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Blocked, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION6", "ACTION7", "ACTION8", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Expired, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Closed, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION9"}
                };
            }
        }

        public static IEnumerable<object[]> CreditWithPINTestDataSet
        {
            get
            {
                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Ordered, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Inactive, true),
                    new List<string>(){"ACTION2", "ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Active, true),
                    new List<string>(){"ACTION1", "ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Restricted, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION5", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Blocked, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION7", "ACTION8", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Expired, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION5", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Closed, true),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION5", "ACTION9"}
                };
            }
        }

        public static IEnumerable<object[]> PrepaidWithoutPINTestDataSet
        {
            get
            {
                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Ordered, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Inactive, false),
                    new List<string>(){"ACTION2", "ACTION3", "ACTION4", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Active, false),
                    new List<string>(){"ACTION1", "ACTION3", "ACTION4", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Restricted, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Blocked, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION8", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Expired, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Prepaid, CardStatus.Closed, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION9"}
                };
            }
        }

        public static IEnumerable<object[]> DebitWithoutPINTestDataSet
        {
            get
            {
                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Ordered, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Inactive, false),
                    new List<string>(){"ACTION2", "ACTION3", "ACTION4", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Active, false),
                    new List<string>(){"ACTION1", "ACTION3", "ACTION4", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Restricted, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Blocked, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION8", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Expired, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Debit, CardStatus.Closed, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION9"}
                };
            }
        }

        public static IEnumerable<object[]> CreditWithoutPINTestDataSet
        {
            get
            {
                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Ordered, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION5", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Inactive, false),
                    new List<string>(){"ACTION2", "ACTION3", "ACTION4", "ACTION5", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Active, false),
                    new List<string>(){"ACTION1", "ACTION3", "ACTION4", "ACTION5", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Restricted, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION5", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Blocked, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION5", "ACTION8", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Expired, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION5", "ACTION9"}
                };

                yield return new object[]
                {
                    new CardDetails("Card1", CardType.Credit, CardStatus.Closed, false),
                    new List<string>(){"ACTION3", "ACTION4", "ACTION5", "ACTION9"}
                };
            }
        }

        [Theory]
        [MemberData(nameof(PDFExamplesTestDataSet))]
        [MemberData(nameof(PrepaidWithPINTestDataSet))]
        [MemberData(nameof(DebitWithPINTestDataSet))]
        [MemberData(nameof(CreditWithPINTestDataSet))]
        [MemberData(nameof(PrepaidWithoutPINTestDataSet))]
        [MemberData(nameof(DebitWithoutPINTestDataSet))]
        [MemberData(nameof(CreditWithoutPINTestDataSet))]
        public void AllTestsShouldPass(CardDetails card, List<string> expected)
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ActionsService>>();
            IActionsService service = new ActionsService(loggerMock.Object);

            // Act
            var output = service.GetAllowedActions(card);

            // Assert
            Assert.Equal(expected, output);
        }
    }
}