import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useEffect, useState } from 'react';
import { fetchCategories, convert } from './api';
const CATEGORY_ICONS = {
    length: '📏',
    weight: '⚖️',
    temperature: '🌡️',
    volume: '🧪',
    speed: '🚀',
    area: '📐',
    energy: '⚡',
};
export default function App() {
    const [categories, setCategories] = useState([]);
    const [selectedCategory, setSelectedCategory] = useState('');
    const [value, setValue] = useState('');
    const [fromUnit, setFromUnit] = useState('');
    const [toUnit, setToUnit] = useState('');
    const [result, setResult] = useState(null);
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
    function handleCategoryChange(catId) {
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
    async function handleConvert(e) {
        e.preventDefault();
        if (!value || !fromUnit || !toUnit)
            return;
        setLoading(true);
        setError('');
        setResult(null);
        try {
            const res = await convert(Number(value), fromUnit, toUnit);
            setResult(res);
        }
        catch (err) {
            setError(err instanceof Error ? err.message : 'Conversion failed');
        }
        finally {
            setLoading(false);
        }
    }
    if (fetchError) {
        return (_jsxs("div", { className: "error-screen", children: [_jsx("span", { className: "error-icon", children: "\u26A0\uFE0F" }), _jsx("p", { children: fetchError })] }));
    }
    return (_jsxs("div", { className: "app", children: [_jsxs("header", { className: "header", children: [_jsx("h1", { children: "Unit Converter" }), _jsx("p", { children: "Convert values across length, weight, temperature and more" })] }), _jsxs("main", { className: "main", children: [_jsx("div", { className: "category-tabs", role: "tablist", children: categories.map((cat) => (_jsxs("button", { role: "tab", "aria-selected": selectedCategory === cat.id, className: `tab ${selectedCategory === cat.id ? 'tab--active' : ''}`, onClick: () => handleCategoryChange(cat.id), children: [_jsx("span", { className: "tab-icon", children: CATEGORY_ICONS[cat.id] ?? '🔢' }), _jsx("span", { className: "tab-label", children: cat.name })] }, cat.id))) }), _jsxs("div", { className: "card", children: [_jsxs("form", { onSubmit: handleConvert, noValidate: true, children: [_jsx("div", { className: "form-row", children: _jsxs("div", { className: "field", children: [_jsx("label", { htmlFor: "value", children: "Value" }), _jsx("input", { id: "value", type: "number", placeholder: "Enter a number", value: value, onChange: (e) => { setValue(e.target.value); setResult(null); }, required: true, "aria-required": "true" })] }) }), _jsxs("div", { className: "unit-row", children: [_jsxs("div", { className: "field", children: [_jsx("label", { htmlFor: "from-unit", children: "From" }), _jsx("select", { id: "from-unit", value: fromUnit, onChange: (e) => { setFromUnit(e.target.value); setResult(null); }, children: activeCategory?.units.map((u) => (_jsx("option", { value: u.id, children: u.name }, u.id))) })] }), _jsx("button", { type: "button", className: "swap-btn", onClick: swapUnits, "aria-label": "Swap units", title: "Swap units", children: "\u21C4" }), _jsxs("div", { className: "field", children: [_jsx("label", { htmlFor: "to-unit", children: "To" }), _jsx("select", { id: "to-unit", value: toUnit, onChange: (e) => { setToUnit(e.target.value); setResult(null); }, children: activeCategory?.units.map((u) => (_jsx("option", { value: u.id, children: u.name }, u.id))) })] })] }), _jsx("button", { type: "submit", className: "convert-btn", disabled: loading || !value, children: loading ? 'Converting…' : 'Convert' })] }), error && (_jsx("div", { className: "alert alert--error", role: "alert", children: error })), result && (_jsxs("div", { className: "result", role: "region", "aria-live": "polite", "aria-label": "Conversion result", children: [_jsxs("div", { className: "result-equation", children: [_jsxs("span", { className: "result-input", children: [result.inputValue, " ", _jsx("em", { children: result.fromUnitName })] }), _jsx("span", { className: "result-equals", children: "=" }), _jsxs("span", { className: "result-output", children: [formatNumber(result.outputValue), " ", _jsx("em", { children: result.toUnitName })] })] }), _jsx("p", { className: "result-category", children: result.category })] }))] })] })] }));
}
function formatNumber(n) {
    if (Math.abs(n) >= 1e9 || (Math.abs(n) < 1e-4 && n !== 0)) {
        return n.toExponential(6);
    }
    // Up to 8 significant digits, strip trailing zeros
    return parseFloat(n.toPrecision(8)).toString();
}
