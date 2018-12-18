var CryptoJS = CryptoJS ||
    function (t, e) {
        var r = {},
            n = r.lib = {},
            i = n.Base = function () {
                function t() { }
                return {
                    extend: function (e) {
                        t.prototype = this;
                        var r = new t;
                        return e && r.mixIn(e),
                            r.$super = this,
                            r
                    },
                    create: function () {
                        var t = this.extend();
                        return t.init.apply(t, arguments),
                            t
                    },
                    init: function () { },
                    mixIn: function (t) {
                        for (var e in t) t.hasOwnProperty(e) && (this[e] = t[e]);
                        t.hasOwnProperty("toString") && (this.toString = t.toString)
                    },
                    clone: function () {
                        return this.$super.extend(this)
                    }
                }
            }(),
            o = n.WordArray = i.extend({
                init: function (t, e) {
                    t = this.words = t || [],
                        this.sigBytes = void 0 != e ? e : 4 * t.length
                },
                toString: function (t) {
                    return (t || a).stringify(this)
                },
                concat: function (t) {
                    var e = this.words,
                        r = t.words,
                        n = this.sigBytes,
                        t = t.sigBytes;
                    if (this.clamp(), n % 4) for (var i = 0; i < t; i++) e[n + i >>> 2] |= (r[i >>> 2] >>> 24 - i % 4 * 8 & 255) << 24 - (n + i) % 4 * 8;
                    else if (65535 < r.length) for (i = 0; i < t; i += 4) e[n + i >>> 2] = r[i >>> 2];
                    else e.push.apply(e, r);
                    return this.sigBytes += t,
                        this
                },
                clamp: function () {
                    var e = this.words,
                        r = this.sigBytes;
                    e[r >>> 2] &= 4294967295 << 32 - r % 4 * 8,
                        e.length = t.ceil(r / 4)
                },
                clone: function () {
                    var t = i.clone.call(this);
                    return t.words = this.words.slice(0),
                        t
                },
                random: function (e) {
                    for (var r = [], n = 0; n < e; n += 4) r.push(4294967296 * t.random() | 0);
                    return o.create(r, e)
                }
            }),
            s = r.enc = {},
            a = s.Hex = {
                stringify: function (t) {
                    for (var e = t.words, t = t.sigBytes, r = [], n = 0; n < t; n++) {
                        var i = e[n >>> 2] >>> 24 - n % 4 * 8 & 255;
                        r.push((i >>> 4).toString(16)),
                            r.push((15 & i).toString(16))
                    }
                    return r.join("")
                },
                parse: function (t) {
                    for (var e = t.length, r = [], n = 0; n < e; n += 2) r[n >>> 3] |= parseInt(t.substr(n, 2), 16) << 24 - n % 8 * 4;
                    return o.create(r, e / 2)
                }
            },
            c = s.Latin1 = {
                stringify: function (t) {
                    for (var e = t.words, t = t.sigBytes, r = [], n = 0; n < t; n++) r.push(String.fromCharCode(e[n >>> 2] >>> 24 - n % 4 * 8 & 255));
                    return r.join("")
                },
                parse: function (t) {
                    for (var e = t.length, r = [], n = 0; n < e; n++) r[n >>> 2] |= (255 & t.charCodeAt(n)) << 24 - n % 4 * 8;
                    return o.create(r, e)
                }
            },
            u = s.Utf8 = {
                stringify: function (t) {
                    try {
                        return decodeURIComponent(escape(c.stringify(t)))
                    } catch (t) {
                        throw Error("Malformed UTF-8 data")
                    }
                },
                parse: function (t) {
                    return c.parse(unescape(encodeURIComponent(t)))
                }
            },
            f = n.BufferedBlockAlgorithm = i.extend({
                reset: function () {
                    this._data = o.create(),
                        this._nDataBytes = 0
                },
                _append: function (t) {
                    "string" == typeof t && (t = u.parse(t)),
                        this._data.concat(t),
                        this._nDataBytes += t.sigBytes
                },
                _process: function (e) {
                    var r = this._data,
                        n = r.words,
                        i = r.sigBytes,
                        s = this.blockSize,
                        a = i / (4 * s),
                        a = e ? t.ceil(a) : t.max((0 | a) - this._minBufferSize, 0),
                        e = a * s,
                        i = t.min(4 * e, i);
                    if (e) {
                        for (var c = 0; c < e; c += s) this._doProcessBlock(n, c);
                        c = n.splice(0, e),
                            r.sigBytes -= i
                    }
                    return o.create(c, i)
                },
                clone: function () {
                    var t = i.clone.call(this);
                    return t._data = this._data.clone(),
                        t
                },
                _minBufferSize: 0
            });
        n.Hasher = f.extend({
            init: function () {
                this.reset()
            },
            reset: function () {
                f.reset.call(this),
                    this._doReset()
            },
            update: function (t) {
                return this._append(t),
                    this._process(),
                    this
            },
            finalize: function (t) {
                return t && this._append(t),
                    this._doFinalize(),
                    this._hash
            },
            clone: function () {
                var t = f.clone.call(this);
                return t._hash = this._hash.clone(),
                    t
            },
            blockSize: 16,
            _createHelper: function (t) {
                return function (e, r) {
                    return t.create(r).finalize(e)
                }
            },
            _createHmacHelper: function (t) {
                return function (e, r) {
                    return h.HMAC.create(t, r).finalize(e)
                }
            }
        });
        var h = r.algo = {};
        return r
    }(Math);
!
    function () {
        var t = CryptoJS,
            e = t.lib.WordArray;
        t.enc.Base64 = {
            stringify: function (t) {
                var e = t.words,
                    r = t.sigBytes,
                    n = this._map;
                t.clamp();
                for (var t = [], i = 0; i < r; i += 3) for (var o = (e[i >>> 2] >>> 24 - i % 4 * 8 & 255) << 16 | (e[i + 1 >>> 2] >>> 24 - (i + 1) % 4 * 8 & 255) << 8 | e[i + 2 >>> 2] >>> 24 - (i + 2) % 4 * 8 & 255, s = 0; 4 > s && i + .75 * s < r; s++) t.push(n.charAt(o >>> 6 * (3 - s) & 63));
                if (e = n.charAt(64)) for (; t.length % 4;) t.push(e);
                return t.join("")
            },
            parse: function (t) {
                var t = t.replace(/\s/g, ""),
                    r = t.length,
                    n = this._map,
                    i = n.charAt(64);
                i && -1 != (i = t.indexOf(i)) && (r = i);
                for (var i = [], o = 0, s = 0; s < r; s++) if (s % 4) {
                    var a = n.indexOf(t.charAt(s - 1)) << s % 4 * 2,
                        c = n.indexOf(t.charAt(s)) >>> 6 - s % 4 * 2;
                    i[o >>> 2] |= (a | c) << 24 - o % 4 * 8,
                        o++
                }
                return e.create(i, o)
            },
            _map: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/="
        }
    }(),


    function (t) {
        function e(t, e, r, n, i, o, s) {
            return ((t = t + (e & r | ~e & n) + i + s) << o | t >>> 32 - o) + e
        }
        function r(t, e, r, n, i, o, s) {
            return ((t = t + (e & n | r & ~n) + i + s) << o | t >>> 32 - o) + e
        }
        function n(t, e, r, n, i, o, s) {
            return ((t = t + (e ^ r ^ n) + i + s) << o | t >>> 32 - o) + e
        }
        function i(t, e, r, n, i, o, s) {
            return ((t = t + (r ^ (e | ~n)) + i + s) << o | t >>> 32 - o) + e
        }
        var o = CryptoJS,
            s = o.lib,
            a = s.WordArray,
            s = s.Hasher,
            c = o.algo,
            u = [];
        !
            function () {
                for (var e = 0; 64 > e; e++) u[e] = 4294967296 * t.abs(t.sin(e + 1)) | 0
            }(),
            c = c.MD5 = s.extend({
                _doReset: function () {
                    this._hash = a.create([1732584193, 4023233417, 2562383102, 271733878])
                },
                _doProcessBlock: function (t, o) {
                    for (var s = 0; 16 > s; s++) {
                        var a = o + s,
                            c = t[a];
                        t[a] = 16711935 & (c << 8 | c >>> 24) | 4278255360 & (c << 24 | c >>> 8)
                    }
                    for (var a = this._hash.words, c = a[0], f = a[1], h = a[2], d = a[3], s = 0; 64 > s; s += 4) 16 > s ? (c = e(c, f, h, d, t[o + s], 7, u[s]), d = e(d, c, f, h, t[o + s + 1], 12, u[s + 1]), h = e(h, d, c, f, t[o + s + 2], 17, u[s + 2]), f = e(f, h, d, c, t[o + s + 3], 22, u[s + 3])) : 32 > s ? (c = r(c, f, h, d, t[o + (s + 1) % 16], 5, u[s]), d = r(d, c, f, h, t[o + (s + 6) % 16], 9, u[s + 1]), h = r(h, d, c, f, t[o + (s + 11) % 16], 14, u[s + 2]), f = r(f, h, d, c, t[o + s % 16], 20, u[s + 3])) : 48 > s ? (c = n(c, f, h, d, t[o + (3 * s + 5) % 16], 4, u[s]), d = n(d, c, f, h, t[o + (3 * s + 8) % 16], 11, u[s + 1]), h = n(h, d, c, f, t[o + (3 * s + 11) % 16], 16, u[s + 2]), f = n(f, h, d, c, t[o + (3 * s + 14) % 16], 23, u[s + 3])) : (c = i(c, f, h, d, t[o + 3 * s % 16], 6, u[s]), d = i(d, c, f, h, t[o + (3 * s + 7) % 16], 10, u[s + 1]), h = i(h, d, c, f, t[o + (3 * s + 14) % 16], 15, u[s + 2]), f = i(f, h, d, c, t[o + (3 * s + 5) % 16], 21, u[s + 3]));
                    a[0] = a[0] + c | 0,
                        a[1] = a[1] + f | 0,
                        a[2] = a[2] + h | 0,
                        a[3] = a[3] + d | 0
                },
                _doFinalize: function () {
                    var t = this._data,
                        e = t.words,
                        r = 8 * this._nDataBytes,
                        n = 8 * t.sigBytes;
                    for (e[n >>> 5] |= 128 << 24 - n % 32, e[14 + (n + 64 >>> 9 << 4)] = 16711935 & (r << 8 | r >>> 24) | 4278255360 & (r << 24 | r >>> 8), t.sigBytes = 4 * (e.length + 1), this._process(), t = this._hash.words, e = 0; 4 > e; e++) r = t[e],
                        t[e] = 16711935 & (r << 8 | r >>> 24) | 4278255360 & (r << 24 | r >>> 8)
                }
            }),
            o.MD5 = s._createHelper(c),
            o.HmacMD5 = s._createHmacHelper(c)
    }(Math),


    function () {
        var t = CryptoJS,
            e = t.lib,
            r = e.Base,
            n = e.WordArray,
            e = t.algo,
            i = e.EvpKDF = r.extend({
                cfg: r.extend({
                    keySize: 4,
                    hasher: e.MD5,
                    iterations: 1
                }),
                init: function (t) {
                    this.cfg = this.cfg.extend(t)
                },
                compute: function (t, e) {
                    for (var r = this.cfg, i = r.hasher.create(), o = n.create(), s = o.words, a = r.keySize, r = r.iterations; s.length < a;) {
                        c && i.update(c);
                        var c = i.update(t).finalize(e);
                        i.reset();
                        for (var u = 1; u < r; u++) c = i.finalize(c),
                            i.reset();
                        o.concat(c)
                    }
                    return o.sigBytes = 4 * a,
                        o
                }
            });
        t.EvpKDF = function (t, e, r) {
            return i.create(r).compute(t, e)
        }
    }(),
    CryptoJS.lib.Cipher ||
    function (t) {
        var e = CryptoJS,
            r = e.lib,
            n = r.Base,
            i = r.WordArray,
            o = r.BufferedBlockAlgorithm,
            s = e.enc.Base64,
            a = e.algo.EvpKDF,
            c = r.Cipher = o.extend({
                cfg: n.extend(),
                createEncryptor: function (t, e) {
                    return this.create(this._ENC_XFORM_MODE, t, e)
                },
                createDecryptor: function (t, e) {
                    return this.create(this._DEC_XFORM_MODE, t, e)
                },
                init: function (t, e, r) {
                    this.cfg = this.cfg.extend(r),
                        this._xformMode = t,
                        this._key = e,
                        this.reset()
                },
                reset: function () {
                    o.reset.call(this),
                        this._doReset()
                },
                process: function (t) {
                    return this._append(t),
                        this._process()
                },
                finalize: function (t) {
                    return t && this._append(t),
                        this._doFinalize()
                },
                keySize: 4,
                ivSize: 4,
                _ENC_XFORM_MODE: 1,
                _DEC_XFORM_MODE: 2,
                _createHelper: function () {
                    return function (t) {
                        return {
                            encrypt: function (e, r, n) {
                                return ("string" == typeof r ? p : l).encrypt(t, e, r, n)
                            },
                            decrypt: function (e, r, n) {
                                return ("string" == typeof r ? p : l).decrypt(t, e, r, n)
                            }
                        }
                    }
                }()
            });
        r.StreamCipher = c.extend({
            _doFinalize: function () {
                return this._process(!0)
            },
            blockSize: 1
        });
        var u = e.mode = {},
            f = r.BlockCipherMode = n.extend({
                createEncryptor: function (t, e) {
                    return this.Encryptor.create(t, e)
                },
                createDecryptor: function (t, e) {
                    return this.Decryptor.create(t, e)
                },
                init: function (t, e) {
                    this._cipher = t,
                        this._iv = e
                }
            }),
            u = u.CBC = function () {
                function e(e, r, n) {
                    var i = this._iv;
                    i ? this._iv = t : i = this._prevBlock;
                    for (var o = 0; o < n; o++) e[r + o] ^= i[o]
                }
                var r = f.extend();
                return r.Encryptor = r.extend({
                    processBlock: function (t, r) {
                        var n = this._cipher,
                            i = n.blockSize;
                        e.call(this, t, r, i),
                            n.encryptBlock(t, r),
                            this._prevBlock = t.slice(r, r + i)
                    }
                }),
                    r.Decryptor = r.extend({
                        processBlock: function (t, r) {
                            var n = this._cipher,
                                i = n.blockSize,
                                o = t.slice(r, r + i);
                            n.decryptBlock(t, r),
                                e.call(this, t, r, i),
                                this._prevBlock = o
                        }
                    }),
                    r
            }(),
            h = (e.pad = {}).Pkcs7 = {
                pad: function (t, e) {
                    for (var r = 4 * e, r = r - t.sigBytes % r, n = r << 24 | r << 16 | r << 8 | r, o = [], s = 0; s < r; s += 4) o.push(n);
                    r = i.create(o, r),
                        t.concat(r)
                },
                unpad: function (t) {
                    t.sigBytes -= 255 & t.words[t.sigBytes - 1 >>> 2]
                }
            };
        r.BlockCipher = c.extend({
            cfg: c.cfg.extend({
                mode: u,
                padding: h
            }),
            reset: function () {
                c.reset.call(this);
                var t = this.cfg,
                    e = t.iv,
                    t = t.mode;
                if (this._xformMode == this._ENC_XFORM_MODE) var r = t.createEncryptor;
                else r = t.createDecryptor,
                    this._minBufferSize = 1;
                this._mode = r.call(t, this, e && e.words)
            },
            _doProcessBlock: function (t, e) {
                this._mode.processBlock(t, e)
            },
            _doFinalize: function () {
                var t = this.cfg.padding;
                if (this._xformMode == this._ENC_XFORM_MODE) {
                    t.pad(this._data, this.blockSize);
                    var e = this._process(!0)
                } else e = this._process(!0),
                    t.unpad(e);
                return e
            },
            blockSize: 4
        });
        var d = r.CipherParams = n.extend({
            init: function (t) {
                this.mixIn(t)
            },
            toString: function (t) {
                return (t || this.formatter).stringify(this)
            }
        }),
            u = (e.format = {}).OpenSSL = {
                stringify: function (t) {
                    var e = t.ciphertext,
                        t = t.salt,
                        e = (t ? i.create([1398893684, 1701076831]).concat(t).concat(e) : e).toString(s);
                    return e = e.replace(/(.{64})/g, "$1\n")
                },
                parse: function (t) {
                    var t = s.parse(t),
                        e = t.words;
                    if (1398893684 == e[0] && 1701076831 == e[1]) {
                        var r = i.create(e.slice(2, 4));
                        e.splice(0, 4),
                            t.sigBytes -= 16
                    }
                    return d.create({
                        ciphertext: t,
                        salt: r
                    })
                }
            },
            l = r.SerializableCipher = n.extend({
                cfg: n.extend({
                    format: u
                }),
                encrypt: function (t, e, r, n) {
                    var n = this.cfg.extend(n),
                        i = t.createEncryptor(r, n),
                        e = i.finalize(e),
                        i = i.cfg;
                    return d.create({
                        ciphertext: e,
                        key: r,
                        iv: i.iv,
                        algorithm: t,
                        mode: i.mode,
                        padding: i.padding,
                        blockSize: t.blockSize,
                        formatter: n.format
                    })
                },
                decrypt: function (t, e, r, n) {
                    return n = this.cfg.extend(n),
                        e = this._parse(e, n.format),
                        t.createDecryptor(r, n).finalize(e.ciphertext)
                },
                _parse: function (t, e) {
                    return "string" == typeof t ? e.parse(t) : t
                }
            }),
            e = (e.kdf = {}).OpenSSL = {
                compute: function (t, e, r, n) {
                    return n || (n = i.random(8)),
                        t = a.create({
                            keySize: e + r
                        }).compute(t, n),
                        r = i.create(t.words.slice(e), 4 * r),
                        t.sigBytes = 4 * e,
                        d.create({
                            key: t,
                            iv: r,
                            salt: n
                        })
                }
            },
            p = r.PasswordBasedCipher = l.extend({
                cfg: l.cfg.extend({
                    kdf: e
                }),
                encrypt: function (t, e, r, n) {
                    return n = this.cfg.extend(n),
                        r = n.kdf.compute(r, t.keySize, t.ivSize),
                        n.iv = r.iv,
                        t = l.encrypt.call(this, t, e, r.key, n),
                        t.mixIn(r),
                        t
                },
                decrypt: function (t, e, r, n) {
                    return n = this.cfg.extend(n),
                        e = this._parse(e, n.format),
                        r = n.kdf.compute(r, t.keySize, t.ivSize, e.salt),
                        n.iv = r.iv,
                        l.decrypt.call(this, t, e, r.key, n)
                }
            })
    }(),


    function () {
        var t = CryptoJS,
            e = t.lib.BlockCipher,
            r = t.algo,
            n = [],
            i = [],
            o = [],
            s = [],
            a = [],
            c = [],
            u = [],
            f = [],
            h = [],
            d = [];
        !
            function () {
                for (var t = [], e = 0; 256 > e; e++) t[e] = 128 > e ? e << 1 : e << 1 ^ 283;
                for (var r = 0, l = 0, e = 0; 256 > e; e++) {
                    var p = l ^ l << 1 ^ l << 2 ^ l << 3 ^ l << 4,
                        p = p >>> 8 ^ 255 & p ^ 99;
                    n[r] = p,
                        i[p] = r;
                    var g = t[r],
                        y = t[g],
                        v = t[y],
                        _ = 257 * t[p] ^ 16843008 * p;
                    o[r] = _ << 24 | _ >>> 8,
                        s[r] = _ << 16 | _ >>> 16,
                        a[r] = _ << 8 | _ >>> 24,
                        c[r] = _,
                        _ = 16843009 * v ^ 65537 * y ^ 257 * g ^ 16843008 * r,
                        u[p] = _ << 24 | _ >>> 8,
                        f[p] = _ << 16 | _ >>> 16,
                        h[p] = _ << 8 | _ >>> 24,
                        d[p] = _,
                        r ? (r = g ^ t[t[t[v ^ g]]], l ^= t[t[l]]) : r = l = 1
                }
            }();
        var l = [0, 1, 2, 4, 8, 16, 32, 64, 128, 27, 54],
            r = r.AES = e.extend({
                _doReset: function () {
                    for (var t = this._key, e = t.words, r = t.sigBytes / 4, t = 4 * ((this._nRounds = r + 6) + 1), i = this._keySchedule = [], o = 0; o < t; o++) if (o < r) i[o] = e[o];
                    else {
                        var s = i[o - 1];
                        o % r ? 6 < r && 4 == o % r && (s = n[s >>> 24] << 24 | n[s >>> 16 & 255] << 16 | n[s >>> 8 & 255] << 8 | n[255 & s]) : (s = s << 8 | s >>> 24, s = n[s >>> 24] << 24 | n[s >>> 16 & 255] << 16 | n[s >>> 8 & 255] << 8 | n[255 & s], s ^= l[o / r | 0] << 24),
                            i[o] = i[o - r] ^ s
                    }
                    for (e = this._invKeySchedule = [], r = 0; r < t; r++) o = t - r,
                        s = r % 4 ? i[o] : i[o - 4],
                        e[r] = 4 > r || 4 >= o ? s : u[n[s >>> 24]] ^ f[n[s >>> 16 & 255]] ^ h[n[s >>> 8 & 255]] ^ d[n[255 & s]]
                },
                encryptBlock: function (t, e) {
                    this._doCryptBlock(t, e, this._keySchedule, o, s, a, c, n)
                },
                decryptBlock: function (t, e) {
                    var r = t[e + 1];
                    t[e + 1] = t[e + 3],
                        t[e + 3] = r,
                        this._doCryptBlock(t, e, this._invKeySchedule, u, f, h, d, i),
                        r = t[e + 1],
                        t[e + 1] = t[e + 3],
                        t[e + 3] = r
                },
                _doCryptBlock: function (t, e, r, n, i, o, s, a) {
                    for (var c = this._nRounds, u = t[e] ^ r[0], f = t[e + 1] ^ r[1], h = t[e + 2] ^ r[2], d = t[e + 3] ^ r[3], l = 4, p = 1; p < c; p++) var g = n[u >>> 24] ^ i[f >>> 16 & 255] ^ o[h >>> 8 & 255] ^ s[255 & d] ^ r[l++],
                        y = n[f >>> 24] ^ i[h >>> 16 & 255] ^ o[d >>> 8 & 255] ^ s[255 & u] ^ r[l++],
                        v = n[h >>> 24] ^ i[d >>> 16 & 255] ^ o[u >>> 8 & 255] ^ s[255 & f] ^ r[l++],
                        d = n[d >>> 24] ^ i[u >>> 16 & 255] ^ o[f >>> 8 & 255] ^ s[255 & h] ^ r[l++],
                        u = g,
                        f = y,
                        h = v;
                    g = (a[u >>> 24] << 24 | a[f >>> 16 & 255] << 16 | a[h >>> 8 & 255] << 8 | a[255 & d]) ^ r[l++],
                        y = (a[f >>> 24] << 24 | a[h >>> 16 & 255] << 16 | a[d >>> 8 & 255] << 8 | a[255 & u]) ^ r[l++],
                        v = (a[h >>> 24] << 24 | a[d >>> 16 & 255] << 16 | a[u >>> 8 & 255] << 8 | a[255 & f]) ^ r[l++],
                        d = (a[d >>> 24] << 24 | a[u >>> 16 & 255] << 16 | a[f >>> 8 & 255] << 8 | a[255 & h]) ^ r[l++],
                        t[e] = g,
                        t[e + 1] = y,
                        t[e + 2] = v,
                        t[e + 3] = d
                },
                keySize: 8
            });
        t.AES = e._createHelper(r)
    }();
var CryptoJS = CryptoJS ||
    function (t, e) {
        var r = {},
            n = r.lib = {},
            i = n.Base = function () {
                function t() { }
                return {
                    extend: function (e) {
                        t.prototype = this;
                        var r = new t;
                        return e && r.mixIn(e),
                            r.$super = this,
                            r
                    },
                    create: function () {
                        var t = this.extend();
                        return t.init.apply(t, arguments),
                            t
                    },
                    init: function () { },
                    mixIn: function (t) {
                        for (var e in t) t.hasOwnProperty(e) && (this[e] = t[e]);
                        t.hasOwnProperty("toString") && (this.toString = t.toString)
                    },
                    clone: function () {
                        return this.$super.extend(this)
                    }
                }
            }(),
            o = n.WordArray = i.extend({
                init: function (t, e) {
                    t = this.words = t || [],
                        this.sigBytes = void 0 != e ? e : 4 * t.length
                },
                toString: function (t) {
                    return (t || a).stringify(this)
                },
                concat: function (t) {
                    var e = this.words,
                        r = t.words,
                        n = this.sigBytes,
                        t = t.sigBytes;
                    if (this.clamp(), n % 4) for (var i = 0; i < t; i++) e[n + i >>> 2] |= (r[i >>> 2] >>> 24 - i % 4 * 8 & 255) << 24 - (n + i) % 4 * 8;
                    else if (65535 < r.length) for (i = 0; i < t; i += 4) e[n + i >>> 2] = r[i >>> 2];
                    else e.push.apply(e, r);
                    return this.sigBytes += t,
                        this
                },
                clamp: function () {
                    var e = this.words,
                        r = this.sigBytes;
                    e[r >>> 2] &= 4294967295 << 32 - r % 4 * 8,
                        e.length = t.ceil(r / 4)
                },
                clone: function () {
                    var t = i.clone.call(this);
                    return t.words = this.words.slice(0),
                        t
                },
                random: function (e) {
                    for (var r = [], n = 0; n < e; n += 4) r.push(4294967296 * t.random() | 0);
                    return o.create(r, e)
                }
            }),
            s = r.enc = {},
            a = s.Hex = {
                stringify: function (t) {
                    for (var e = t.words, t = t.sigBytes, r = [], n = 0; n < t; n++) {
                        var i = e[n >>> 2] >>> 24 - n % 4 * 8 & 255;
                        r.push((i >>> 4).toString(16)),
                            r.push((15 & i).toString(16))
                    }
                    return r.join("")
                },
                parse: function (t) {
                    for (var e = t.length, r = [], n = 0; n < e; n += 2) r[n >>> 3] |= parseInt(t.substr(n, 2), 16) << 24 - n % 8 * 4;
                    return o.create(r, e / 2)
                }
            },
            c = s.Latin1 = {
                stringify: function (t) {
                    for (var e = t.words, t = t.sigBytes, r = [], n = 0; n < t; n++) r.push(String.fromCharCode(e[n >>> 2] >>> 24 - n % 4 * 8 & 255));
                    return r.join("")
                },
                parse: function (t) {
                    for (var e = t.length, r = [], n = 0; n < e; n++) r[n >>> 2] |= (255 & t.charCodeAt(n)) << 24 - n % 4 * 8;
                    return o.create(r, e)
                }
            },
            u = s.Utf8 = {
                stringify: function (t) {
                    try {
                        return decodeURIComponent(escape(c.stringify(t)))
                    } catch (t) {
                        throw Error("Malformed UTF-8 data")
                    }
                },
                parse: function (t) {
                    return c.parse(unescape(encodeURIComponent(t)))
                }
            },
            f = n.BufferedBlockAlgorithm = i.extend({
                reset: function () {
                    this._data = o.create(),
                        this._nDataBytes = 0
                },
                _append: function (t) {
                    "string" == typeof t && (t = u.parse(t)),
                        this._data.concat(t),
                        this._nDataBytes += t.sigBytes
                },
                _process: function (e) {
                    var r = this._data,
                        n = r.words,
                        i = r.sigBytes,
                        s = this.blockSize,
                        a = i / (4 * s),
                        a = e ? t.ceil(a) : t.max((0 | a) - this._minBufferSize, 0),
                        e = a * s,
                        i = t.min(4 * e, i);
                    if (e) {
                        for (var c = 0; c < e; c += s) this._doProcessBlock(n, c);
                        c = n.splice(0, e),
                            r.sigBytes -= i
                    }
                    return o.create(c, i)
                },
                clone: function () {
                    var t = i.clone.call(this);
                    return t._data = this._data.clone(),
                        t
                },
                _minBufferSize: 0
            });
        n.Hasher = f.extend({
            init: function () {
                this.reset()
            },
            reset: function () {
                f.reset.call(this),
                    this._doReset()
            },
            update: function (t) {
                return this._append(t),
                    this._process(),
                    this
            },
            finalize: function (t) {
                return t && this._append(t),
                    this._doFinalize(),
                    this._hash
            },
            clone: function () {
                var t = f.clone.call(this);
                return t._hash = this._hash.clone(),
                    t
            },
            blockSize: 16,
            _createHelper: function (t) {
                return function (e, r) {
                    return t.create(r).finalize(e)
                }
            },
            _createHmacHelper: function (t) {
                return function (e, r) {
                    return h.HMAC.create(t, r).finalize(e)
                }
            }
        });
        var h = r.algo = {};
        return r
    }(Math);
!
    function () {
        var t = CryptoJS,
            e = t.lib,
            r = e.WordArray,
            e = e.Hasher,
            n = [],
            i = t.algo.SHA1 = e.extend({
                _doReset: function () {
                    this._hash = r.create([1732584193, 4023233417, 2562383102, 271733878, 3285377520])
                },
                _doProcessBlock: function (t, e) {
                    for (var r = this._hash.words, i = r[0], o = r[1], s = r[2], a = r[3], c = r[4], u = 0; 80 > u; u++) {
                        if (16 > u) n[u] = 0 | t[e + u];
                        else {
                            var f = n[u - 3] ^ n[u - 8] ^ n[u - 14] ^ n[u - 16];
                            n[u] = f << 1 | f >>> 31
                        }
                        f = (i << 5 | i >>> 27) + c + n[u],
                            f = 20 > u ? f + (1518500249 + (o & s | ~o & a)) : 40 > u ? f + (1859775393 + (o ^ s ^ a)) : 60 > u ? f + ((o & s | o & a | s & a) - 1894007588) : f + ((o ^ s ^ a) - 899497514),
                            c = a,
                            a = s,
                            s = o << 30 | o >>> 2,
                            o = i,
                            i = f
                    }
                    r[0] = r[0] + i | 0,
                        r[1] = r[1] + o | 0,
                        r[2] = r[2] + s | 0,
                        r[3] = r[3] + a | 0,
                        r[4] = r[4] + c | 0
                },
                _doFinalize: function () {
                    var t = this._data,
                        e = t.words,
                        r = 8 * this._nDataBytes,
                        n = 8 * t.sigBytes;
                    e[n >>> 5] |= 128 << 24 - n % 32,
                        e[15 + (n + 64 >>> 9 << 4)] = r,
                        t.sigBytes = 4 * e.length,
                        this._process()
                }
            });
        t.SHA1 = e._createHelper(i),
            t.HmacSHA1 = e._createHmacHelper(i)
    }(),


    function () {
        var t = CryptoJS,
            e = t.enc.Utf8;
        t.algo.HMAC = t.lib.Base.extend({
            init: function (t, r) {
                t = this._hasher = t.create(),
                    "string" == typeof r && (r = e.parse(r));
                var n = t.blockSize,
                    i = 4 * n;
                r.sigBytes > i && (r = t.finalize(r));
                for (var o = this._oKey = r.clone(), s = this._iKey = r.clone(), a = o.words, c = s.words, u = 0; u < n; u++) a[u] ^= 1549556828,
                    c[u] ^= 909522486;
                o.sigBytes = s.sigBytes = i,
                    this.reset()
            },
            reset: function () {
                var t = this._hasher;
                t.reset(),
                    t.update(this._iKey)
            },
            update: function (t) {
                return this._hasher.update(t),
                    this
            },
            finalize: function (t) {
                var e = this._hasher,
                    t = e.finalize(t);
                return e.reset(),
                    e.finalize(this._oKey.clone().concat(t))
            }
        })
    }(),


    function () {
        var t = CryptoJS,
            e = t.lib,
            r = e.Base,
            n = e.WordArray,
            e = t.algo,
            i = e.HMAC,
            o = e.PBKDF2 = r.extend({
                cfg: r.extend({
                    keySize: 4,
                    hasher: e.SHA1,
                    iterations: 1
                }),
                init: function (t) {
                    this.cfg = this.cfg.extend(t)
                },
                compute: function (t, e) {
                    for (var r = this.cfg, o = i.create(r.hasher, t), s = n.create(), a = n.create([1]), c = s.words, u = a.words, f = r.keySize, r = r.iterations; c.length < f;) {
                        var h = o.update(e).finalize(a);
                        o.reset();
                        for (var d = h.words, l = d.length, p = h, g = 1; g < r; g++) {
                            p = o.finalize(p),
                                o.reset();
                            for (var y = p.words, v = 0; v < l; v++) d[v] ^= y[v]
                        }
                        s.concat(h),
                            u[0]++
                    }
                    return s.sigBytes = 4 * f,
                        s
                }
            });
        t.PBKDF2 = function (t, e, r) {
            return o.create(r).compute(t, e)
        }
    }();
var wd = wd || {};
wd.login = wd.login || {};
var warnings = $(".warning"),
    timer;
wd.login.init = function () {
    $(document).keydown(function (t) {
        var e = window.event || t;
        13 == (e.keyCode || e.which) && $("#login-btn").submit()
    }),
        $(".forget-password").click(function () {
            window.location.href = "/my-account/findPassword"
        }),
        $("#login-btn").click(function (t) {
            if ($(this).attr("disabled")) return !1;
            t.preventDefault(),
                wd.login.doLogin()
        }),
        $(".email-input").focus(function () {
            warnings.eq(0).removeClass("warning-show"),
                $(this).removeClass("warning-border")
        }).blur(function () {
            if ("" !== $(this).val() && !wd.util.validateEmail($(this).val())) return $(".warning").eq(0).addClass("warning-show"),
                void $(this).addClass("warning-border")
        }),
        $(".password-input").focus(function () {
            warnings.eq(1).removeClass("warning-show"),
                $(this).removeClass("warning-border")
        }),
        $("#auto-login").click(function () {
            "false" == $(this).attr("data-autologin") ? ($(this).parents(".pro").css("color", "rgb( 50, 175, 90)"), $(this).attr("data-autologin", !0), $(this).find("img").attr("src", "https://cdn.wilddog.com/www-nd/images/account/checkbox-color-3fc5da9271.svg")) : ($(this).parents(".pro").css("color", "rgb( 102, 102, 102 )"), $(this).attr("data-autologin", !1), $(this).find("img").attr("src", "https://cdn.wilddog.com/www-nd/images/account/checkbox-nor-dd70fc394a.svg"))
        })
},
    wd.login.doLogin = function () {
        var t = $("input[name=email]").val(),
            e = $("input[name=password]").val(),
            r = $("#auto-login").attr("data-autoLogin");
        if (!wd.util.validateEmail(t)) return warnings.eq(0).addClass("warning-show"),
            void $(".email-input").addClass("warning-border");
        if (!wd.util.validatePassword(e)) return warnings.eq(1).addClass("warning-show"),
            void $(".password-input").addClass("warning-border");
        var n = $("#next").val();
        null != n && "" != n && "/my-account/activation" != n || (n = "/dashboard"),
            $("#login-btn").attr("disabled", "true");
        var i = $("#sk").val(),
            o = $("#ck").val(),
            s = CryptoJS.lib.WordArray.random(16).toString(CryptoJS.enc.Hex),
            a = CryptoJS.lib.WordArray.random(16).toString(CryptoJS.enc.Hex),
            c = new AesUtil(128, 1e3),
            u = c.encrypt(a, s, i, e);
        t = t.toLowerCase();
        var f = {
            email: t,
            password: [o, a, s, u].join("."),
            autologin: r
        };
        $.ajax({
            url: "/wwwapi/account/login",
            type: "POST",
            data: f,
            cache: !1,
            timeout: 3e4,
            dataType: "json",
            success: function (e, r, i) {
                var o = e.code;
                window.location.href = "/dashboard";
                    
            },
            error: function () {
                $("#login-btn").removeAttr("disabled"),
                    warnings.eq(2).addClass("warning-show"),
                    setTimeout(function () {
                        warnings.eq(2).removeClass("warning-show")
                    }, 2e3)
            }
        })
    },
    wd.login.doGithub = function () {
        $("#login-btn").attr("disabled", "true");
        var t;
        if ("undefined" != typeof mixpanel) try {
            mixpanel.track("login with github"),
                t = mixpanel.get_distinct_id()
        } catch (t) {
            console.warn(t)
        }
        $.ajax({
            url: "/wwwapi/account/github/url",
            type: "GET",
            data: {
                distinctId: t
            },
            dataType: "json",
            success: function (t, e, r) {
                var n = t.url;
                setTimeout(window.location = n, 200)
            },
            error: function () {
                $("#login-btn").removeAttr("disabled")
            }
        })
    },
    wd.util = wd.util || {},
    wd.util.validateEmail = function (t) {
        return /\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/.test(t)
    },
    wd.util.validatePassword = function (t) {
        return t.length >= 8
    };
var AesUtil = function (t, e) {
    this.keySize = t / 32,
        this.iterationCount = e
};
AesUtil.prototype.generateKey = function (t, e) {
    return CryptoJS.PBKDF2(e, CryptoJS.enc.Hex.parse(t), {
        keySize: this.keySize,
        iterations: this.iterationCount
    })
},
    AesUtil.prototype.encrypt = function (t, e, r, n) {
        var i = this.generateKey(t, r);
        return CryptoJS.AES.encrypt(n, i, {
            iv: CryptoJS.enc.Hex.parse(e)
        }).ciphertext.toString(CryptoJS.enc.Base64)
    },
    AesUtil.prototype.decrypt = function (t, e, r, n) {
        var i = this.generateKey(t, r),
            o = CryptoJS.lib.CipherParams.create({
                ciphertext: CryptoJS.enc.Base64.parse(n)
            });
        return CryptoJS.AES.decrypt(o, i, {
            iv: CryptoJS.enc.Hex.parse(e)
        }).toString(CryptoJS.enc.Utf8)
    };