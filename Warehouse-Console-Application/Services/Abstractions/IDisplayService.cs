using System.Collections.Generic;
using System;

public interface IDisplayService {
    void DisplayPalletsGroupedByExpiryDate(List<Pallet> pallets);
    void DisplayTop3PalletsWithMaxExpiryDate(List<Pallet> pallets);
}
