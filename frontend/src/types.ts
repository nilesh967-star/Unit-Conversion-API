export interface UnitInfo {
  id: string;
  name: string;
}

export interface Category {
  id: string;
  name: string;
  units: UnitInfo[];
}

export interface ConversionResult {
  inputValue: number;
  fromUnit: string;
  fromUnitName: string;
  outputValue: number;
  toUnit: string;
  toUnitName: string;
  category: string;
}
