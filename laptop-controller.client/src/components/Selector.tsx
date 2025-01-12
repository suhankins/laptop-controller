import { useCallback } from 'react';

export default function Selector({
    action,
    name,
    options,
}: {
    action: string;
    name: string;
    options: string[];
}) {
    const sendValue = useCallback((value: string) => {
        const url = action + '?' + new URLSearchParams({ [name]: value });
        fetch(url, {
            method: 'POST',
        });
    }, [action, name]);

    return (
        <div>
            {options.map((option, index) => (
                <button onClick={() => sendValue(index.toString())} key={option}>
                    {option}
                </button>
            ))}
        </div>
    );
}
