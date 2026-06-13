import { useEffect, useState } from 'react';
import { fetchCategories, convert } from './api';
import type { Category, ConversionResult } from './types';

const CATEGORY_ICONS: Record<string, string> = {
  length: '📏',
  weight: '⚖️',
  temperature: '🌡️',
  volume: '🧪',
  speed: '🚀',
  area: '📐',
  energy: '⚡',
};

export default function App() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [selectedCategory, setSelectedCategory] = useState('');
  const [value, setValue] = useState('');
  const [fromUnit, setFromUnit] = useState('');
  const [toUnit, setToUnit] = useState('');
  const [result, setResult] = useState<ConversionResult | null>(null);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [fetchError, setFetchError] = useState('');

  useEffect(() => {
    fetchCategories()
      .then((cats) => {
        setCategories(cats);
        if (cats.length > 0) {
          setSelectedCategory(cats[0].id);
          setFromUnit(cats[0].units[0]?.id ?? '');
          setToUnit(cats[0].units[1]?.id ?? '');
        }
      })
      .catch(() => setFetchError('Could not reach the API. Make sure the backend is running on http://localhost:5000.'));
  }, []);

  const activeCategory = categories.find((c) => c.id === selectedCategory);

  function handleCategoryChange(catId: string) {
    setSelectedCategory(catId);
    setResult(null);
    setError('');
    const cat = categories.find((c) => c.id === catId);
    setFromUnit(cat?.units[0]?.id ?? '');
    setToUnit(cat?.units[1]?.id ?? '');
  }

  function swapUnits() {
    setFromUnit(toUnit);
    setToUnit(fromUnit);
    setResult(null);
  }

  async function handleConvert(e: React.FormEvent) {
    e.preventDefault();
    if (!value || !fromUnit || !toUnit) return;
    setLoading(true);
    setError('');
    setResult(null);
    try {
      const res = await convert(Number(value), fromUnit, toUnit);
      setResult(res);
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : 'Conversion failed');
    } finally {
      setLoading(false);
    }
  }

  if (fetchError) {
    return (
      <div className="error-screen">
        <span className="error-icon">⚠️</span>
        <p>{fetchError}</p>
      </div>
    );
  }

  return (
    <div className="app">
      <header className="header">
        <h1>Unit Converter</h1>
        <p>Convert values across length, weight, temperature and more</p>
      </header>

      <main className="main">
        {/* Category tabs */}
        <div className="category-tabs" role="tablist">
          {categories.map((cat) => (
            <button
              key={cat.id}
              role="tab"
              aria-selected={selectedCategory === cat.id}
              className={`tab ${selectedCategory === cat.id ? 'tab--active' : ''}`}
              onClick={() => handleCategoryChange(cat.id)}
            >
              <span className="tab-icon">{CATEGORY_ICONS[cat.id] ?? '🔢'}</span>
              <span className="tab-label">{cat.name}</span>
            </button>
          ))}
        </div>

        {/* Converter card */}
        <div className="card">
          <form onSubmit={handleConvert} noValidate>
            <div className="form-row">
              {/* Value input */}
              <div className="field">
                <label htmlFor="value">Value</label>
                <input
                  id="value"
                  type="number"
                  placeholder="Enter a number"
                  value={value}
                  onChange={(e) => { setValue(e.target.value); setResult(null); }}
                  required
                  aria-required="true"
                />
              </div>
            </div>

            <div className="unit-row">
              {/* From unit */}
              <div className="field">
                <label htmlFor="from-unit">From</label>
                <select
                  id="from-unit"
                  value={fromUnit}
                  onChange={(e) => { setFromUnit(e.target.value); setResult(null); }}
                >
                  {activeCategory?.units.map((u) => (
                    <option key={u.id} value={u.id}>{u.name}</option>
                  ))}
                </select>
              </div>

              {/* Swap button */}
              <button
                type="button"
                className="swap-btn"
                onClick={swapUnits}
                aria-label="Swap units"
                title="Swap units"
              >
                ⇄
              </button>

              {/* To unit */}
              <div className="field">
                <label htmlFor="to-unit">To</label>
                <select
                  id="to-unit"
                  value={toUnit}
                  onChange={(e) => { setToUnit(e.target.value); setResult(null); }}
                >
                  {activeCategory?.units.map((u) => (
                    <option key={u.id} value={u.id}>{u.name}</option>
                  ))}
                </select>
              </div>
            </div>

            <button
              type="submit"
              className="convert-btn"
              disabled={loading || !value}
            >
              {loading ? 'Converting…' : 'Convert'}
            </button>
          </form>

          {/* Error */}
          {error && (
            <div className="alert alert--error" role="alert">
              {error}
            </div>
          )}

          {/* Result */}
          {result && (
            <div className="result" role="region" aria-live="polite" aria-label="Conversion result">
              <div className="result-equation">
                <span className="result-input">
                  {result.inputValue} <em>{result.fromUnitName}</em>
                </span>
                <span className="result-equals">=</span>
                <span className="result-output">
                  {formatNumber(result.outputValue)} <em>{result.toUnitName}</em>
                </span>
              </div>
              <p className="result-category">{result.category}</p>
            </div>
          )}
        </div>
      </main>
    </div>
  );
}

function formatNumber(n: number): string {
  if (Math.abs(n) >= 1e9 || (Math.abs(n) < 1e-4 && n !== 0)) {
    return n.toExponential(6);
  }
  // Up to 8 significant digits, strip trailing zeros
  return parseFloat(n.toPrecision(8)).toString();
}
