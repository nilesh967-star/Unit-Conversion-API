import type { Category, ConversionResult } from './types';

const BASE = '/api/conversion';

export async function fetchCategories(): Promise<Category[]> {
  const res = await fetch(`${BASE}/units`);
  if (!res.ok) throw new Error('Failed to load units');
  return res.json();
}

export async function convert(
  value: number,
  fromUnit: string,
  toUnit: string
): Promise<ConversionResult> {
  const res = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ value, fromUnit, toUnit }),
  });
  if (!res.ok) {
    const err = await res.json();
    throw new Error(err.detail ?? 'Conversion failed');
  }
  return res.json();
}
