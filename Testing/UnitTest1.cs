using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace WarehouseTests {
    [TestClass]
    public class WarehouseTests {
        // Тест 1: Проверка, что дата истечения срока годности устанавливается на 100 дней позже даты производства коробки.
        [TestMethod]
        public void Box_ProductionDateSetsExpiryDate() {
            DateTime productionDate = new DateTime(2023, 1, 1);
            var box = new Box("B1", 10, 10, 10, 5, productionDate: productionDate);

            DateTime? expectedExpiryDate = productionDate.AddDays(100);

            Assert.AreEqual(expectedExpiryDate, box.ExpiryDate);
        }

        // Тест 2: Проверка, что добавление коробки увеличивает вес паллеты.
        [TestMethod]
        public void Pallet_AddBox_IncreasesWeight() {
            var pallet = new Pallet("P1", 120, 100, 15);
            var box1 = new Box("B1", 30, 20, 10, 5, DateTime.Now);
            var box2 = new Box("B2", 30, 20, 10, 7, DateTime.Now);
            
            pallet.AddBox(box1);
            pallet.AddBox(box2);
            double expectedWeight = 30 + box1.Weight + box2.Weight;

            Assert.AreEqual(expectedWeight, pallet.Weight);
        }


        // Тест 3: Проверка расчета общего объема паллеты с учетом коробок.
        [TestMethod]
        public void Pallet_CalculateTotalVolume_ReturnsCorrectVolume() {
            var pallet = new Pallet("P1", 120, 100, 15);
            var box1 = new Box("B1", 30, 20, 10, 5, DateTime.Now);
            var box2 = new Box("B2", 30, 20, 10, 7, DateTime.Now);
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            double expectedVolume = pallet.CalculateVolume() + box1.CalculateVolume() + box2.CalculateVolume();
            double actualVolume = pallet.CalculateTotalVolume();

            Assert.AreEqual(expectedVolume, actualVolume);
        }


        // Тест 4: Проверка, что добавление коробки с размерами больше паллеты выбрасывает исключение.
        [TestMethod]
        public void Pallet_AddBox_ThrowsExceptionWhenBoxExceedsPalletSize() {
            var pallet = new Pallet("P1", 50, 50, 50);
            var oversizedBox = new Box("B1", 60, 20, 60, 5, DateTime.Now);

            Assert.ThrowsException<InvalidOperationException>(() => pallet.AddBox(oversizedBox));
        }


        // Тест 5: Проверка, что срок годности паллеты - это минимальный срок годности среди коробок.
        [TestMethod]
        public void Pallet_ExpiryDate_IsMinimumExpiryOfBoxes() {
            var pallet = new Pallet("P1", 120, 100, 15);
            var box1 = new Box("B1", 30, 20, 10, 5, expiryDate: new DateTime(2024, 1, 1));
            var box2 = new Box("B2", 30, 20, 10, 7, expiryDate: new DateTime(2023, 12, 15));
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            DateTime? expectedExpiryDate = new DateTime(2023, 12, 15);

            Assert.AreEqual(expectedExpiryDate, pallet.ExpiryDate);
        }

        // Тест 6: Проверка группировки паллет по сроку годности.
        [TestMethod]
        public void PalletService_GroupPalletsByExpiryDate() {
            var pallet1 = new Pallet("P1", 120, 100, 15);
            var box1 = new Box("B1", 30, 20, 10, 5, expiryDate: new DateTime(2023, 12, 15));
            pallet1.AddBox(box1);

            var pallet2 = new Pallet("P2", 120, 100, 15);
            var box2 = new Box("B2", 30, 20, 10, 5, expiryDate: new DateTime(2024, 1, 1));
            pallet2.AddBox(box2);

            var pallets = new List<Pallet> { pallet1, pallet2 };

            var groupedPallets = pallets
                .OrderBy(p => p.ExpiryDate)
                .GroupBy(p => p.ExpiryDate)
                .ToList();

            Assert.AreEqual(2, groupedPallets.Count);
            Assert.AreEqual(1, groupedPallets.First().Count());
            Assert.AreEqual(1, groupedPallets.Last().Count());
        }
    }
}
