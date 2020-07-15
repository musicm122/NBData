module NBData.Models

type RevenueQueryData = {
    Year: string    
    Description: string
}

type BudgetRow =
    { Id: int
      FiscalYear: string
      RevenueCode: int
      Description: string
      Income: float
      Budget: float
      Projected: float
      Proposed: float
      ProjectedFiscalYear: string }

let filterBudgetRow(year:string) =
    