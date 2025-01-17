import { useCallback } from 'react';
import styles from './Selector.module.css';

export default function Selector({
    action,
    name,
    options,
    children,
}: {
    action: string;
    name: string;
    options: string[];
    children: React.ReactNode;
}) {
    const sendValue = useCallback(
        (value: string) => {
            const url = action + '?' + new URLSearchParams({ [name]: value });
            fetch(url, {
                method: 'POST',
            });
        },
        [action, name]
    );

    return (
        <div role="group" aria-labelledby={`group_${action}`}>
            <p id={`group_${action}`} className={styles.header}>
                {children}
            </p>
            <div className={styles.innerGroup}>
                {options.map((option, index) => (
                    <button
                        onClick={() => sendValue(index.toString())}
                        key={option}>
                        {option}
                    </button>
                ))}
            </div>
        </div>
    );
}
