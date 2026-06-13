const BASE = '/api/conversion';
export async function fetchCategories() {
    const res = await fetch(`${BASE}/units`);
    if (!res.ok)
        throw new Error('Failed to load units');
    return res.json();
}
export async function convert(value, fromUnit, toUnit) {
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
