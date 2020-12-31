// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Castle.Components.DictionaryAdapter.Xml;
// using ClassLibrary1.Mocking;
// using Moq;
// using NUnit.Framework;
//
// namespace ClassLibrary1.UnitTests.Mocking
// {
//     [TestFixture]
//     public class HousekeeperHelperTests
//     {
//         private HousekeeperService _service;
//         private Mock<IUnitOfWork> _work;
//         private Mock<IStatementGenerator> _generator;
//         private Mock<IEmailSender> _sender;
//         private Mock<IXtraMessageBox> _box;
//         private readonly DateTime _statementDate = new DateTime(2020, 12, 12);
//
//         [SetUp]
//         public void SetUp()
//         {
//             _work = new Mock<IUnitOfWork>();
//             _generator = new Mock<IStatementGenerator>();
//             _sender = new Mock<IEmailSender>();
//             _box = new Mock<IXtraMessageBox>();
//             _service = new HousekeeperService(_work.Object, _generator.Object, _sender.Object, _box.Object);
//         }
//
//         [Test]
//         public void SendStatementEmails_EmailIsNull_SaveStatementAndEmailFileAndShowWontVerifyThenReturnTrue()
//         {
//             _work.Setup(w => w.Query<Housekeeper>()).Returns(new List<Housekeeper>
//             {
//                 new Housekeeper {Email = null, Oid = 1, FullName = "a", StatementEmailBody = "a"}
//             }.AsQueryable());
//
//             var result = _service.SendStatementEmails(_statementDate);
//
//             _generator.Verify(g => g.SaveStatement(1, "a", _statementDate), Times.Never);
//             _sender.Verify(s => s.EmailFile("a", "a", "a", "a"), Times.Never);
//             _box.Verify(b => b.Show("a", "a", MessageBoxButtons.OK), Times.Never);
//             Assert.That(result, Is.True);
//         }
//
//         [Test]
//         [TestCase(null)]
//         [TestCase("")]
//         [TestCase(" ")]
//         public void
//             SendStatementEmails_EmailIsStringButStatementFilenameIsNullOrWhiteSpace_SaveStatementVerifyButEmailFileAndShowWontVerifyThenReturnTrue(
//                 string statementFilename)
//         {
//             _work.Setup(w => w.Query<Housekeeper>()).Returns(new List<Housekeeper>
//             {
//                 new Housekeeper {Email = "a", Oid = 1, FullName = "a", StatementEmailBody = "a"}
//             }.AsQueryable());
//             _generator.Setup(g => g.SaveStatement(1, "a", _statementDate)).Returns(statementFilename);
//
//             var result = _service.SendStatementEmails(_statementDate);
//             _generator.Verify(g => g.SaveStatement(1, "a", _statementDate));
//             _sender.Verify(s => s.EmailFile("a", "a", "a", "a"), Times.Never);
//             _box.Verify(b => b.Show("a", "a", MessageBoxButtons.OK), Times.Never);
//             Assert.That(result, Is.True);
//         }
//
//         [Test]
//         public void
//             SendStatementEmails_EmailIsStringAndStatementFilenameIsString_SaveStatementAndEmailFileVerifyButShowWontVerifyThenReturnTrue()
//         {
//             _work.Setup(w => w.Query<Housekeeper>()).Returns(new List<Housekeeper>
//             {
//                 new Housekeeper {Email = "a", Oid = 1, FullName = "a", StatementEmailBody = "a"}
//             }.AsQueryable());
//             _generator.Setup(g => g.SaveStatement(1, "a", _statementDate)).Returns("a");
//
//             var result = _service.SendStatementEmails(_statementDate);
//
//             _generator.Verify(g => g.SaveStatement(1, "a", _statementDate));
//             _sender.Verify(s => s.EmailFile("a", "a", "a", "a"));
//             _box.Verify(b => b.Show("a", "a", MessageBoxButtons.OK), Times.Never);
//             Assert.That(result, Is.True);
//         }
//
//         [Test]
//         public void
//             SendStatementEmails_EmailIsStringAndStatementFilenameIsStringButThrowException_SaveStatementAndEmailFileVerifyThenShowWontVerify()
//         {
//             _work.Setup(w => w.Query<Housekeeper>()).Returns(new List<Housekeeper>
//             {
//                 new Housekeeper {Email = "a", Oid = 1, FullName = "a", StatementEmailBody = "a"}
//             }.AsQueryable());
//             _generator.Setup(g => g.SaveStatement(1, "a", _statementDate)).Returns("a");
//             _sender.Setup(s => s.EmailFile("a", "a", "a", "a")).Throws<Exception>();
//
//             Assert.That(() => _service.SendStatementEmails(_statementDate), Throws.Exception);
//         }
//     }
// }