import './App.css';
import Selector from './components/Selector/Selector';
import Slider from './components/Slider/Slider';

function App() {
    return (
        <div>
            <h1>Laptop Controller</h1>
            <p>It genuinenly makes me paranoid I'm gonna brick it somehow.</p>
            <Slider action="screenbrightness" name="brightness">
                Screen Brightness
            </Slider>
            <Slider action="volume" name="volume">
                Volume
            </Slider>
            <Selector
                action="keyboardbrightness"
                name="brightness"
                options={['Off', 'Low', 'Mid', 'Max']}>
                Keyboard Brightness
            </Selector>
        </div>
    );
}

export default App;
