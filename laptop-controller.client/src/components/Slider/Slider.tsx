import { useEffect, useMemo, useState } from 'react';
import debounce from 'lodash/debounce';
import styles from './Slider.module.css';

type SliderProps =
    | {
          min?: number;
          max?: number;
          step?: number;
          action: string;
          name: string;
          children: React.ReactNode;
      } & React.InputHTMLAttributes<HTMLInputElement>;

export default function Slider({
    min = 0,
    max = 100,
    step = 1,
    action,
    name,
    children,
    ...props
}: SliderProps) {
    const [loading, setLoading] = useState(false); // useState(true);
    const [value, setValue] = useState(0);

    const sendValue = useMemo(
        () =>
            debounce((value) => {
                const url =
                    action + '?' + new URLSearchParams({ [name]: value });
                setLoading(true);
                fetch(url, {
                    method: 'POST',
                }).then(() => {
                    setLoading(false);
                });
            }, 500),
        [action, name]
    );

    useEffect(() => {
        fetch(action)
            .then((response) => {
                if (!response.ok) {
                    throw new Error(`Request returned code ${response.status}`);
                }
                return response.json();
            })
            .then((text) => {
                setLoading(false);
                setValue(Number.parseInt(text));
            });
    }, [action]);

    return (
        <label className={styles.container} aria-labelledby={`slider_${action}`}>
            <p id={`slider_${action}`}>{children}</p>
            <div className={styles.sliderContainer}>
                <span className={styles.valueDisplay}>{value}%</span>
                <input
                    className={styles.slider}
                    disabled={loading}
                    type="range"
                    min={min}
                    max={max}
                    step={step}
                    value={value}
                    onChange={(event) => {
                        const parsedValue = Number.parseInt(event.target.value);
                        setValue(parsedValue);
                        sendValue(parsedValue);
                    }}
                    {...props}
                />
            </div>
        </label>
    );
}
